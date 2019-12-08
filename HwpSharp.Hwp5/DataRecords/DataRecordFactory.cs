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
    }
}
