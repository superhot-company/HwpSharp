using SuperHot.HwpSharp.Common.HwpType;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SuperHot.HwpSharp.Hwp5.HwpType
{
    public class HwpReader : BinaryReader
    {
        public HwpReader(Stream input) : base(input, Encoding.Unicode, false)
        {
        }

        public HwpReader(byte[] bytes) : this(new MemoryStream(bytes, false))
        {
        }

        public HwpUnit ReadHwpUnit() => ReadUInt32();

        public SHwpUnit ReadSHwpUnit() => ReadInt32();

        public HwpUnit16 ReadHwpUnit16() => ReadInt16();

        public Color ReadColor() => ReadUInt32();

    }
}
