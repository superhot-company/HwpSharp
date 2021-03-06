﻿using System.Collections.Generic;
using System.IO;
using OpenMcdf;
using SuperHot.HwpSharp.Hwp5.DataRecords;

namespace SuperHot.HwpSharp.Hwp5
{
    public class Section : ISection
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

        public Section(HwpStreamReader reader, FileHeader fileHeader, DocumentInformation docInfo)
        {
            _fileHeader = fileHeader;
            _docInfo = docInfo;
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
