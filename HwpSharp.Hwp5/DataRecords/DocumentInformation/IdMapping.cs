using System;
using System.Collections.Generic;
using SuperHot.HwpSharp.Common;

namespace SuperHot.HwpSharp.Hwp5.DataRecords
{
    public class IdMapping : DataRecord
    {
        public const uint IdMappingsTagId = HwpTagBegin + 1;

        public int[] IdMappingCounts { get; }

        public int BinaryDataCount => IdMappingCounts[0];

        public int KoreanFontCount => IdMappingCounts[1];

        public int EnglishFontCount => IdMappingCounts[2];

        public int ChineseFontCount => IdMappingCounts[3];

        public int JapaneseFontCount => IdMappingCounts[4];

        public int OtherFontCount => IdMappingCounts[5];

        public int SymbolFontCount => IdMappingCounts[6];

        public int UserFontCount => IdMappingCounts[7];

        public int BorderBackgroundCount => IdMappingCounts[8];

        public int CharacterShapeCount => IdMappingCounts[9];

        public int TabDefinitionCount => IdMappingCounts[10];

        public int ParagraphNumberCount => IdMappingCounts[11];

        public int ListHeaderTableCount => IdMappingCounts[12];

        public int ParagraphShapeCount => IdMappingCounts[13];

        public int StyleCount => IdMappingCounts[14];

        // Version >= 5.0.2.1
        public int MemoShapeCount
        {
            get
            {
                if (IdMappingCounts.Length < 15)
                {
                    throw new HwpUnsupportedPropertyException();
                }

                return IdMappingCounts[15];
            }
        }

        // Version >= 5.0.3.2
        public int TrackChangeCount
        {
            get
            {
                if (IdMappingCounts.Length < 16)
                {
                    throw new HwpUnsupportedPropertyException();
                }

                return IdMappingCounts[16];
            }
        }

        // Version >= 5.0.3.2
        public int TrackChangeUserCount
        {
            get
            {
                if (IdMappingCounts.Length < 17)
                {
                    throw new HwpUnsupportedPropertyException();
                }

                return IdMappingCounts[17];
            }
        }

        public IdMapping(uint level, byte[] bytes, FileHeader _ = null, DocumentInformation __ = null)
            : base(IdMappingsTagId, level, (uint) bytes.Length, bytes)
        {
            var mappings = new List<int>();
            using(var reader = new HwpStreamReader(bytes))
            {
                mappings.Add(reader.ReadInt32());
            }
            IdMappingCounts = mappings.ToArray();
        }
    }
}
