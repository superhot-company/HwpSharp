using SuperHot.HwpSharp.Common.HwpType;
using SuperHot.HwpSharp.Hwp5.DataRecords;
using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace SuperHot.HwpSharp.Hwp5
{
    public class HwpReader : BinaryReader
    {
        protected HwpReader(HwpReader reader) : base(reader.BaseStream, Encoding.Unicode, false)
        {
        }

        public HwpReader(byte[] bytes, bool distributed = false, bool compressed = false)
            : base(new MemoryStream(DecodeBytes(bytes, distributed, compressed), false), Encoding.Unicode, false)
        {
        }

        private static byte[] DecodeBytes(byte[] bytes, bool distributed, bool compressed)
        {
            if (distributed)
            {
                bytes = DecodeDistributionBytes(bytes);
            }

            if (compressed)
            {
                bytes = DecodeCompressedBytes(bytes);
            }

            return bytes;
        }

        private static byte[] DecodeCompressedBytes(byte[] bytes)
        {
            using (var dataStream = new MemoryStream(bytes, false))
            {
                using (var zipStream = new DeflateStream(dataStream, CompressionMode.Decompress))
                {
                    using (var decStream = new MemoryStream())
                    {
                        zipStream.CopyTo(decStream);
                        return decStream.ToArray();
                    }
                }
            }
        }

        private static byte[] DecodeDistributionBytes(byte[] bytes)
        {
            uint header = bytes[0] + bytes[1] * 0x100u + bytes[2] * 0x10000u + bytes[3] * 0x1000000u;
            var recordOffset = 4;

            var tagId = header & 0x3FF;
            _ = (header >> 10) & 0x3FF;
            var size = header >> 20;
            if (size == 0xfff)
            {
                size = bytes[4] + bytes[5] * 0x100u + bytes[6] * 0x10000u + bytes[7] * 0x1000000u;
                recordOffset += 4;
            }

            if (tagId != (uint)TagEnum.DistributeDocData)
            {
                throw new ArgumentException("Specified byte array or stream is not a distribution stream");
            }

            var distDocRecordBytes = new byte[size];
            Array.Copy(bytes, recordOffset, distDocRecordBytes, 0, size);

            uint seed = distDocRecordBytes[0] + distDocRecordBytes[1] * 0x100u + distDocRecordBytes[2] * 0x10000u + distDocRecordBytes[3] * 0x1000000u;

            using (var random = new Common.Random(seed).GetEnumerator())
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
                    if (i >= 4)
                    {
                        distDocRecordBytes[i] ^= key;
                    }
                }
            }

            var offset = 4 + (seed & 0x0F);
            var shaKey = new byte[16];
            Array.Copy(distDocRecordBytes, offset, shaKey, 0, shaKey.Length);

            using (
                var aes = new AesManaged
                {
                    KeySize = 128,
                    BlockSize = 128,
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.None
                })
            {
                using (var byteStream = new MemoryStream(bytes, recordOffset + (int)size, bytes.Length - (recordOffset + (int)size), false))
                {
                    using (
                    var cryptoStream = new CryptoStream(byteStream, aes.CreateDecryptor(shaKey, shaKey),
                        CryptoStreamMode.Read))
                    {
                        using (var decryptedStream = new MemoryStream())
                        {
                            cryptoStream.CopyTo(decryptedStream);
                            return decryptedStream.ToArray();
                        }
                    }
                }
            }
        }

        public HwpUnit ReadHwpUnit() => ReadUInt32();

        public SHwpUnit ReadSHwpUnit() => ReadInt32();

        public HwpUnit16 ReadHwpUnit16() => ReadInt16();

        public Color ReadColor() => ReadUInt32();

        public string ReadString(int count)
        {
            if (count == 0)
            {
                return null;
            }
            var bytes = ReadBytes(count * 2);
            return Encoding.Unicode.GetString(bytes);
        }

        public bool IsEndOfStream() => BaseStream.Position == BaseStream.Length;
    }
}
