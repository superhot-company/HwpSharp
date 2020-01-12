using SuperHot.HwpSharp.Common;
using System;
using System.IO;
using System.Security.Cryptography;

namespace SuperHot.HwpSharp.Hwp5.DataRecords
{
    public class DistributeDocData : DataRecord
    {
        public const uint DistributeDocDataTagId = HwpTagBegin + 12;
        public const uint DistributeDocDataLength = 256;

        public byte[] RandomBytes { get; set; } // 256 bytes

        public uint Seed { get; set; }

        public byte[] Sha1Code { get; set; }// 80 bytes

        public byte[] DecodeKey { get; set; }// 16 bytes

        private char flag;

        public bool CopyProtected => (flag & 1) == 1;

        public bool PrintProtected => (flag & 2) == 1;

        public DistributeDocData(uint level, byte[] bytes, FileHeader _ = null, DocumentInformation __ = null)
            : base(DistributeDocDataTagId, level, (uint) bytes.Length, bytes)
        {
            if (bytes.Length != 256)
            {
                throw new HwpDataRecordConstructorException();
            }

            RandomBytes = new byte[bytes.Length];

            Seed = bytes[0] + bytes[1] * 0x100u + bytes[2] * 0x10000u + bytes[3] * 0x1000000u;

            using(var random = new Common.Random(Seed).GetEnumerator())
            {
                uint n = 0;
                byte key = 0;
                for (var i = 0; i < 256; ++i, --n)
                {
                    if (n == 0)
                    {
                        random.MoveNext();
                        key = (byte)(random.Current & 0xFF);
                        random.MoveNext();
                        n = (random.Current & 0xF) + 1;
                    }
                    RandomBytes[i] = key;
                }
            }

            var offset = 4 + (Seed & 0x0F);
            for (var i = 0; i < 256; ++i)
            {
                RandomBytes[i] ^= bytes[i];
            }

            Sha1Code = new byte[80];
            Array.Copy(RandomBytes, offset, Sha1Code, 0, 80);
            flag = (char)(RandomBytes[offset + 80] + RandomBytes[offset + 81] * 0x100u);

            DecodeKey = new byte[16];
            Array.Copy(Sha1Code, 0, DecodeKey, 0, 16);
        }

        public Stream GetDistributionDecodeStream(Stream stream)
        {
            using (
                var aes = new AesManaged
                {
                    KeySize = 128,
                    BlockSize = 128,
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.None
                })
            {
                return new CryptoStream(stream, aes.CreateDecryptor(DecodeKey, DecodeKey), CryptoStreamMode.Read);
            }
        }
    }
}
