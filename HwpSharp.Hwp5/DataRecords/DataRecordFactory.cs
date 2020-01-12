using SuperHot.HwpSharp.Common;
using System;
using System.Collections.Generic;

namespace SuperHot.HwpSharp.Hwp5.DataRecords
{
    public static class DataRecordFactory
    {
        private static readonly Dictionary<uint, Type> DataRecordTypes = new Dictionary<uint, Type>
        {
            {DocumentProperty.DocumentPropertiesTagId, typeof (DocumentProperty)},
            {IdMapping.IdMappingsTagId, typeof (IdMapping)},
            {BinData.BinDataTagId, typeof (BinData)},
            {FaceName.FaceNameTagId, typeof (FaceName)},
            {BorderFill.BorderFillTagId, typeof (BorderFill)},

            {ParagraphHeader.ParagraphHeaderTagId, typeof (ParagraphHeader)},
            {ParagraphText.ParagraphTextTagId, typeof (ParagraphText)},

            {DistributeDocData.DistributeDocDataTagId, typeof(DistributeDocData)},
        };

        public static void RegisterType(uint tagId, Type type)
        {
            if (DataRecordTypes.ContainsKey(tagId))
            {
                throw new InvalidOperationException($"Already exists {tagId}");
            }
            DataRecordTypes[tagId] = type;
        }

        public static void UnregisterType(uint tagId)
        {
            DataRecordTypes.Remove(tagId);
        }

        public static DataRecord Create(uint tagId, uint level, uint size, byte[] data, FileHeader fileHeader = null, DocumentInformation docInfo = null)
        {
            if (size != data.Length)
            {
                throw new ArgumentException("size != data.Length");
            }

            if (!DataRecordTypes.ContainsKey(tagId))
            {
                return new UnknownDataRecord(tagId, level, data);
            }

            var ctor =
                DataRecordTypes[tagId].GetConstructor(new[]
                {typeof (uint), typeof (byte[]), typeof (FileHeader), typeof (DocumentInformation)});
            if (ctor == null)
            {
                throw new HwpDataRecordConstructorException();
            }
            return ctor.Invoke(new object[] { level, data, fileHeader, docInfo }) as DataRecord;
        }

        public static (uint, uint, uint, bool) ParseHeader(byte byte0, byte byte1, byte byte2, byte byte3)
        {
            uint header = byte0 + byte1 * 0x100u + byte2 * 0x10000u + byte3 * 0x1000000u;
            return ParseHeader(header);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="header"></param>
        /// <returns>(tagId, level, size, needAdditionalByte)</returns>
        public static (uint, uint, uint, bool) ParseHeader(uint header)
        {
            uint tagId = header & 0x3FF;
            uint level = (header >> 10) & 0x3FF;
            uint size = header >> 20;
            var needAdditionalByte = size == 0xFFF;

            return (tagId, level, size, needAdditionalByte);
        }
    }
}
