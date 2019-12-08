using System.Collections.Generic;

namespace SuperHot.HwpSharp.Hwp5
{
    public class BinaryData
    {
        private readonly FileHeader _fileHeader;

        private readonly DocumentInformation _docInfo;

        public IDictionary<uint, byte[]> Data { get; }

        public BinaryData(FileHeader fileHeader, DocumentInformation docInfo)
        {
            _fileHeader = fileHeader;
            _docInfo = docInfo;
            Data = new SortedDictionary<uint, byte[]>();
        }

    }
}