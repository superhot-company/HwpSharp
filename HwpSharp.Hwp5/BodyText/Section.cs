using System.Collections.Generic;
using OpenMcdf;
using SuperHot.HwpSharp.Hwp5.DataRecords;

namespace SuperHot.HwpSharp.Hwp5
{
    public partial class BodyText
    {
        public class Section
        {
            private readonly FileHeader _fileHeader;
            private readonly DocumentInformation _docInfo;

            public List<DataRecord> DataRecords { get; }

            public Section(FileHeader fileHeader, DocumentInformation docInfo)
            {
                _fileHeader = fileHeader;
                _docInfo = docInfo;
                DataRecords = new List<DataRecord>();
            }

            public Section(HwpReader reader, FileHeader fileHeader, DocumentInformation docInfo)
            {
                _fileHeader = fileHeader;
                _docInfo = docInfo;
                DataRecords = new List<DataRecord>();

                using (var recordReader = new HwpDataRecordReader(reader, fileHeader, docInfo))
                {
                    while (!recordReader.IsEndOfStream())
                    {
                        var record = recordReader.ReadDataRecord();
                        DataRecords.Add(record);
                    }
                }
            }
        }
    }
}
