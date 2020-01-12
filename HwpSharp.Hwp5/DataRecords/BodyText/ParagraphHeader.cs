using System;
using System.IO;

namespace SuperHot.HwpSharp.Hwp5.DataRecords
{
    public class ParagraphHeader : DataRecord
    {
        public const uint ParagraphHeaderTagId = HwpTagBegin + 50;

        [Flags]
        public enum ColumnKind : byte
        {
            None = 0x00,
            Area = 0x01,
            MultiColumn = 0x02,
            Page = 0x04,
            Column = 0x08
        }

        public uint Length { get; set; }
        public uint ControlMask { get; set; }
        public ushort ParagraphShapeId { get; set; }
        public byte ParagraphStyleId { get; set; }
        public ColumnKind ColumnType { get; set; }
        public ushort CharacterShapeCount { get; set; }
        public ushort ParagraphRangeCount { get; set; }
        public ushort LineAlignCount { get; set; }
        public uint ParagraphId { get; set; }
        public ushort HistoryMergeParagraphFlag { get; set; }

        public ParagraphHeader(uint level, byte[] bytes, FileHeader _ = null, DocumentInformation docInfo = null)
            : base(ParagraphHeaderTagId, level, (uint) bytes.Length, bytes)
        {
            using(var reader = new HwpStreamReader(bytes))
            {
                Length = reader.ReadUInt32();
                if ((Length & 0x80000000u) != 0)
                {
                    Length &= 0x7fffffffu;
                }

                ControlMask = reader.ReadUInt32();

                ParagraphShapeId = reader.ReadUInt16();

                ParagraphStyleId = reader.ReadByte();

                ColumnKind columnKind = Enum.TryParse(reader.ReadByte().ToString(), out columnKind) ? columnKind : ColumnKind.None;
                ColumnType = columnKind;

                CharacterShapeCount = reader.ReadUInt16();

                ParagraphRangeCount = reader.ReadUInt16();

                LineAlignCount = reader.ReadUInt16();

                ParagraphId = reader.ReadUInt32();

                try
                {
                    HistoryMergeParagraphFlag = reader.ReadUInt16();
                }
                catch (EndOfStreamException)
                {
                    // Do Nothing
                }
            }
        }
    }
}
