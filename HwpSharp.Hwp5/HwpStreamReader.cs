using SuperHot.HwpSharp.Common;
using SuperHot.HwpSharp.Common.HwpType;
using SuperHot.HwpSharp.Hwp5.DataRecords;
using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace SuperHot.HwpSharp.Hwp5
{
    public class HwpStreamReader : BinaryReader
    {
        private readonly FileHeader fileHeader;
        private readonly DocumentInformation docInfo;

        public HwpStreamReader(byte[] bytes, bool distributed = false, bool compressed = false, FileHeader fileHeader = null, DocumentInformation docInfo = null)
            : base(GetDecodeStream(bytes, distributed, compressed), Encoding.Unicode, false)
        {
            this.fileHeader = fileHeader;
            this.docInfo = docInfo;
        }

        private static Stream GetDecodeStream(byte[] bytes, bool distributed, bool compressed)
        {
            Stream stream = new MemoryStream(bytes);
            if (distributed)
            {
                var headerBytes = new byte[4];
                stream.Read(headerBytes, 0, 4);
                var header = headerBytes[0] + headerBytes[1] * 0x100u + headerBytes[2] * 0x10000u + headerBytes[3] * 0x1000000u;
                var (tagId, level, _, _) = DataRecordFactory.ParseHeader(header);
                if (tagId != DistributeDocData.DistributeDocDataTagId)
                {
                    throw new HwpCorruptedDataRecordException("Invalid DistributeDocData data record");
                }

                var recordBytes = new byte[256];
                stream.Read(recordBytes, 0, 256);

                var record = new DistributeDocData(level, recordBytes);
                stream = record.GetDistributionDecodeStream(stream);
            }

            if (compressed)
            {
                stream = new DeflateStream(stream, CompressionMode.Decompress);
            }

            return stream;
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

        public DataRecord ReadDataRecord()
        {
            var header = ReadUInt32();

            var (tagId, level, size, flag) = DataRecordFactory.ParseHeader(header);
            if (flag)
            {
                try
                {
                    size = ReadUInt32();
                }
                catch (EndOfStreamException ex)
                {
                    throw new HwpCorruptedDataRecordException("Unexpected end of stream", ex);
                }
            }

            try
            {
                var bytes = ReadBytes((int)size);

                var record = DataRecordFactory.Create(tagId, level, size, bytes, fileHeader, docInfo);

                return record;
            }
            catch (EndOfStreamException ex)
            {
                throw new HwpCorruptedDataRecordException("Unexpected end of stream", ex);
            }
        }
    }
}
