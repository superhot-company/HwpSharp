using System.Collections.Generic;
using SuperHot.HwpSharp.Common;
using SuperHot.HwpSharp.Hwp5.DataRecords;

namespace SuperHot.HwpSharp.Hwp5
{
    /// <summary>
    /// Represents a hwp 5.0 document information.
    /// </summary>
    public class DocumentInformation : IDocumentInformation
    {
        private readonly FileHeader _fileHeader;

        public IList<DataRecord> DataRecords { get; }

        public DocumentProperty DocumentProperty { get; }

        public IdMapping IdMappings { get; }

        public IList<BinData> BinDataList { get; }

        /// <summary>
        /// Creates a blank <see cref="DocumentInformation"/> instance with specified file header.
        /// </summary>
        /// <param name="fileHeader">Hwp 5 file header</param>
        public DocumentInformation(FileHeader fileHeader)
        {
            _fileHeader = fileHeader;
            DataRecords = new List<DataRecord>();
        }

        public DocumentInformation(HwpReader reader, FileHeader fileHeader)
        {
            _fileHeader = fileHeader;
            DataRecords = new List<DataRecord>();
            BinDataList = new List<BinData>();

            using(var recordReader = new HwpDataRecordReader(reader, fileHeader))
            {
                while(!recordReader.IsEndOfStream())
                {
                    var record = recordReader.ReadDataRecord();

                    switch(record.TagId)
                    {
                        case (uint)TagEnum.DocumentProperties:
                            {
                                if (DocumentProperty == null)
                                {
                                    DocumentProperty = (DocumentProperty)record;
                                }
                                else
                                {
                                    throw new HwpCorruptedDocumentInformationException("Duplicated DocumentProperty");
                                }
                            }
                            break;
                        case (uint)TagEnum.IdMappings:
                            {
                                if (IdMappings == null)
                                {
                                    IdMappings = (IdMapping)record;
                                }
                                else
                                {
                                    throw new HwpCorruptedDocumentInformationException("Duplicated IdMapping");
                                }
                            }
                            break;
                        case (uint)TagEnum.BinData:
                            {
                                BinDataList.Add((BinData)record);
                            }
                            break;
                    }

                    DataRecords.Add(record);
                }
            }
        }
    }
}
