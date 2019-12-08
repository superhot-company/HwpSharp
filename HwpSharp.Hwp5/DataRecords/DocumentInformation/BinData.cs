using System;
using System.Diagnostics;
using System.Text;
using SuperHot.HwpSharp.Common;

namespace SuperHot.HwpSharp.Hwp5.DataRecords
{
    public class BinData : DataRecord
    {
        public const uint BinDataTagId = HwpTagBegin + 2;

        public enum TypeProperty : byte
        {
            Link = 0,
            Embedding = 1,
            Storage = 2,
        }

        public enum CompressionProperty : byte
        {
            StorageDefault = 0,
            Compress = 1,
            NotCompress = 2,
        }

        public enum StateProperty : byte
        {
            Never = 0,
            Success = 1,
            Failed = 2,
            Ignored = 3,
        }

        [DebuggerDisplay("Type={Type}, Compression={Compression}, State={State}")]
        public struct BinDataProperty
        {
            public TypeProperty Type { get; }

            public CompressionProperty Compression { get; }

            public StateProperty State { get; }

            public BinDataProperty(ushort value)
            {
                TypeProperty type;
                Type = Enum.TryParse($"{value & 0x7}", out type) ? type : TypeProperty.Link;

                CompressionProperty compression;
                Compression = Enum.TryParse($"{(value >> 4) & 0x3}", out compression)
                    ? compression
                    : CompressionProperty.StorageDefault;

                StateProperty state;
                State = Enum.TryParse($"{(value >> 8) & 0x3}", out state) ? state : StateProperty.Never;
            }

            public BinDataProperty(TypeProperty type, CompressionProperty compression, StateProperty state)
            {
                Type = type;
                Compression = compression;
                State = state;
            }

            public static implicit operator ushort(BinDataProperty binData)
            {
                return (ushort)(((ushort)binData.State << 8) + ((ushort)binData.Compression << 4) + (ushort)binData.Type);
            }
        }

        public BinDataProperty Property { get; }

        private readonly string _linkFileAbsolutePath;
        public string LinkFileAbsolutePath
        {
            get
            {
                if (Property.Type != TypeProperty.Link)
                {
                    throw new HwpUnsupportedPropertyException();
                }
                return _linkFileAbsolutePath;
            }
        }

        private readonly string _linkFileRelativePath;
        public string LinkFileRelativePath
        {
            get
            {
                if (Property.Type != TypeProperty.Link)
                {
                    throw new HwpUnsupportedPropertyException();
                }
                return _linkFileRelativePath;
            }
        }

        private readonly ushort _binaryDataId;
        public ushort BinaryDataId
        {
            get
            {
                if (Property.Type != TypeProperty.Embedding && Property.Type != TypeProperty.Storage)
                {
                    throw new HwpUnsupportedPropertyException();
                }
                return _binaryDataId;
            }
        }

        private readonly string _binaryDataExtension;
        public string BinaryDataExtension
        {
            get
            {
                if (Property.Type != TypeProperty.Embedding)
                {
                    throw new HwpUnsupportedPropertyException();
                }
                return _binaryDataExtension;
            }
        }

        public BinData(uint level, byte[] bytes, FileHeader _ = null, DocumentInformation __ = null)
            : base(BinDataTagId, level, (uint) bytes.Length, bytes)
        {
            using(var reader = new HwpReader(bytes))
            {
                Property = new BinDataProperty(reader.ReadUInt16());

                // Type = LINK
                if (Property.Type == TypeProperty.Link)
                {
                    var absolutePathLength = reader.ReadUInt16();
                    _linkFileAbsolutePath = reader.ReadString(absolutePathLength);

                    var relativePathLength = reader.ReadUInt16();
                    _linkFileRelativePath = reader.ReadString(relativePathLength);
                }

                // Type = EMBEDDING or STORAGE
                if (Property.Type == TypeProperty.Embedding || Property.Type == TypeProperty.Storage)
                {
                    _binaryDataId = reader.ReadUInt16();
                }

                // Type = EMBEDDING
                if (Property.Type == TypeProperty.Embedding)
                {
                    var binaryDataExtensionLength = reader.ReadUInt16();
                    _binaryDataExtension = reader.ReadString(binaryDataExtensionLength);
                }
            }
        }
    }
}
