namespace SuperHot.HwpSharp.Hwp5.DataRecords
{
    /// <summary>
    /// Represents a data record of hwp 5 document.
    /// </summary>
    public abstract class DataRecord
    {
        internal const int HwpTagBegin = 0x10;

        private byte[] data;

        /// <summary>
        /// Gets the tag id of data record.
        /// </summary>
        public uint TagId { get; private set; }

        /// <summary>
        /// Gets the level of data record.
        /// </summary>
        public uint Level { get; internal set; }

        /// <summary>
        /// Gets the size of data record.
        /// </summary>
        public uint Size { get; internal set; }

        public byte[] GetData()
        {
            return data;
        }

        protected DataRecord(uint tagId, uint level, uint size, byte[] data)
        {
            TagId = tagId;
            Level = level;
            Size = size;
            this.data = data;
        }
    }

    public class UnknownDataRecord : DataRecord
    {
        public UnknownDataRecord(uint tagId, uint level, byte[] data)
            : base(tagId, level, (uint)data.Length, data)
        {
        }
    }
}
