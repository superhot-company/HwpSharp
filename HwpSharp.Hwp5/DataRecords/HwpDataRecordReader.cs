using System;

namespace SuperHot.HwpSharp.Hwp5.DataRecords
{
    public class HwpDataRecordReader : HwpReader
    {
        private readonly FileHeader fileHeader;
        private readonly DocumentInformation docInfo;

        public HwpDataRecordReader(HwpReader input, FileHeader fileHeader = null, DocumentInformation docInfo = null) : base(input)
        {
            this.fileHeader = fileHeader;
            this.docInfo = docInfo;
        }

        public DataRecord ReadDataRecord()
        {
            var header = ReadUInt32();

            var tagId = header & 0x3FF;
            var level = (header >> 10) & 0x3FF;
            var size = header >> 20;
            if (size == 0xfff)
            {
                size = ReadUInt32();
            }

            var bytes = ReadBytes((int)size);

            var record = DataRecordFactory.Create(tagId, level, size, bytes, fileHeader, docInfo);

            return record;
        }
    }
}
