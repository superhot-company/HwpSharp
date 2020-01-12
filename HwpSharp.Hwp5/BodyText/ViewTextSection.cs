using System.Collections.Generic;
using System.IO;
using OpenMcdf;
using SuperHot.HwpSharp.Hwp5.DataRecords;

namespace SuperHot.HwpSharp.Hwp5
{
    public class ViewTextSection : ISection, IDistribution
    {
        private readonly FileHeader _fileHeader;
        private readonly DocumentInformation _docInfo;

        public List<DataRecord> DataRecords { get; }

        public DistributeDocData DistributeDocData { get; set; }

        public ViewTextSection(DistributeDocData distributeDocData, FileHeader fileHeader, DocumentInformation docInfo)
        {
            _fileHeader = fileHeader;
            _docInfo = docInfo;
            DistributeDocData = distributeDocData;
            DataRecords = new List<DataRecord>();
        }

        public ViewTextSection(HwpStreamReader reader, DistributeDocData distributeDocData, FileHeader fileHeader, DocumentInformation docInfo)
        {
            _fileHeader = fileHeader;
            _docInfo = docInfo;
            DistributeDocData = distributeDocData;
            DataRecords = new List<DataRecord>();
            while (true)
            {
                try
                {
                    var record = reader.ReadDataRecord();
                    DataRecords.Add(record);
                }
                catch (EndOfStreamException)
                {
                    break;
                }
            }
        }
    }
}
