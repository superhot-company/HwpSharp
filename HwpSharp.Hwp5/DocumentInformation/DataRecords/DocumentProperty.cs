using SuperHot.HwpSharp.Hwp5.HwpType;
using SuperHot.HwpSharp.Common.HwpType;

namespace SuperHot.HwpSharp.Hwp5.DocumentInformation.DataRecords
{
    public class DocumentProperty : DataRecord
    {
        public const uint DocumentPropertiesTagId = HwpTagBegin;

        public ushort SectionCount { get; set; }
        public ushort StartPageNumber { get; set; }
        public ushort StartFootNoteNumber { get; set; }
        public ushort StartEndNoteNumber { get; set; }
        public ushort StartPictureNumber { get; set; }
        public ushort StartTableNumber { get; set; }
        public ushort StartEquationNumber { get; set; }
        public uint ListId { get; set; }
        public uint ParagraphId { get; set; }
        public uint CharacterUnitPosition { get; set; }

        public DocumentProperty(uint level, byte[] bytes, DocumentInformation _ = null)
            : base(DocumentPropertiesTagId, level, (uint) bytes.Length)
        {
            SectionCount = bytes.ToUInt16();
            StartPageNumber = bytes.ToUInt16(2);
            StartFootNoteNumber = bytes.ToUInt16(4);
            StartEndNoteNumber = bytes.ToUInt16(6);
            StartPictureNumber = bytes.ToUInt16(8);
            StartTableNumber = bytes.ToUInt16(10);
            StartEquationNumber = bytes.ToUInt16(12);
            ListId = bytes.ToUInt32(14);
            ParagraphId = bytes.ToUInt32(18);
            CharacterUnitPosition = bytes.ToUInt32(22);
        }
    }
}
