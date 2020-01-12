using SuperHot.HwpSharp.Common;

namespace SuperHot.HwpSharp.Hwp5.DataRecords
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

        public DocumentProperty(uint level, byte[] bytes, FileHeader _ = null, DocumentInformation __ = null)
            : base(DocumentPropertiesTagId, level, (uint) bytes.Length, bytes)
        {
            if (bytes.Length != 26)
            {
                throw new HwpCorruptedDataRecordException("The length of DocumentProperty is not 26.");
            }

            using(var reader = new HwpStreamReader(bytes))
            {
                SectionCount = reader.ReadUInt16();
                StartPageNumber = reader.ReadUInt16();
                StartFootNoteNumber = reader.ReadUInt16();
                StartEndNoteNumber = reader.ReadUInt16();
                StartPictureNumber = reader.ReadUInt16();
                StartTableNumber = reader.ReadUInt16();
                StartEquationNumber = reader.ReadUInt16();
                ListId = reader.ReadUInt32();
                ParagraphId = reader.ReadUInt32();
                CharacterUnitPosition = reader.ReadUInt32();
            }
        }
    }
}
