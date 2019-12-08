using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SuperHot.HwpSharp.Common.HwpType
{
    /// <summary>
    ///     HWP internal unit, represented as 1/1700 inch.
    /// </summary>
    [DebuggerDisplay("{_value}")]
    [DebuggerTypeProxy(typeof(uint))]
    public struct HwpUnit
    {
        private readonly uint _value;

        private HwpUnit(uint value)
        {
            _value = value;
        }

        /// <summary>
        ///     Implicitly converts a <see cref="uint" /> to a <see cref="HwpUnit" />.
        /// </summary>
        /// <param name="value">The <see cref="uint" /> to convert.</param>
        /// <returns>A new <see cref="HwpUnit" /> with the specified value.</returns>
        public static implicit operator HwpUnit(uint value)
        {
            return new HwpUnit(value);
        }

        /// <summary>
        ///     Implicitly converts a <see cref="HwpUnit" /> to a <see cref="uint" />.
        /// </summary>
        /// <param name="value">The <see cref="HwpUnit" /> to convert.</param>
        /// <returns>A <see cref="uint" /> that is the specified <see cref="HwpUnit" />'s value.</returns>
        public static implicit operator uint(HwpUnit value)
        {
            return value._value;
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }

    /// <summary>
    ///     HWP internal unit, represented as 1/1700 inch with sign.
    /// </summary>
    [DebuggerDisplay("{_value}")]
    [DebuggerTypeProxy(typeof(int))]
    public struct SHwpUnit
    {
        private readonly int _value;

        private SHwpUnit(int value)
        {
            _value = value;
        }

        /// <summary>
        ///     Implicitly converts a <see cref="int" /> to a <see cref="SHwpUnit" />.
        /// </summary>
        /// <param name="value">The <see cref="int" /> to convert.</param>
        /// <returns>A new <see cref="SHwpUnit" /> with the specified value.</returns>
        public static implicit operator SHwpUnit(int value)
        {
            return new SHwpUnit(value);
        }

        /// <summary>
        ///     Implicitly converts a <see cref="SHwpUnit" /> to a <see cref="int" />.
        /// </summary>
        /// <param name="value">The <see cref="SHwpUnit" /> to convert.</param>
        /// <returns>A <see cref="int" /> that is the specified <see cref="SHwpUnit" />'s value.</returns>
        public static implicit operator int(SHwpUnit value)
        {
            return value._value;
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }

    /// <summary>
    ///     Same as INT16
    /// </summary>
    [DebuggerDisplay("{_value}")]
    [DebuggerTypeProxy(typeof(short))]
    public struct HwpUnit16
    {
        private readonly short _value;

        private HwpUnit16(short value)
        {
            _value = value;
        }

        /// <summary>
        ///     Implicitly converts a <see cref="short" /> to a <see cref="HwpUnit16" />.
        /// </summary>
        /// <param name="value">The <see cref="short" /> to convert.</param>
        /// <returns>A new <see cref="HwpUnit16" /> with the specified value.</returns>
        public static implicit operator HwpUnit16(short value)
        {
            return new HwpUnit16(value);
        }

        /// <summary>
        ///     Implicitly converts a <see cref="HwpUnit16" /> to a <see cref="short" />.
        /// </summary>
        /// <param name="value">The <see cref="HwpUnit16" /> to convert.</param>
        /// <returns>A <see cref="short" /> that is the specified <see cref="HwpUnit16" />'s value.</returns>
        public static implicit operator short(HwpUnit16 value)
        {
            return value._value;
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }

    /// <summary>
    ///     Represents RGB value(0x00bbggrr) as decimal.
    /// </summary>
    [DebuggerDisplay("({Red}, {Green}, {Blue})")]
    public struct Color
    {
        public byte Red { get; }
        public byte Green { get; }
        public byte Blue { get; }

        public Color(byte red, byte green, byte blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        /// <summary>
        ///     Implicitly converts a <see cref="uint" /> to a <see cref="Color" />.
        /// </summary>
        /// <param name="value">The <see cref="uint" /> to convert.</param>
        /// <returns>A new <see cref="Color" /> with the specified value.</returns>
        public static implicit operator Color(uint value)
        {
            byte red = (byte)(value & 0xff);
            byte green = (byte)((value >> 8) & 0xff);
            byte blue = (byte)((value >> 16) & 0xff);
            return new Color(red, green, blue);
        }

        /// <summary>
        ///     Implicitly converts a <see cref="Color" /> to a <see cref="uint" />.
        /// </summary>
        /// <param name="value">The <see cref="Color" /> to convert.</param>
        /// <returns>A <see cref="uint" /> that is the specified <see cref="Color" />'s value.</returns>
        public static implicit operator uint(Color value)
        {
            return value.Red + value.Green*0x100u + value.Blue*0x10000u;
        }

        public override string ToString()
        {
            return $"({Red}, {Green}, {Blue})";
        }
    }

    public static class ByteArrayConverter
    {
        public static ushort ToWord(this IEnumerable<byte> bytes, int offset = 0)
        {
            var arr = bytes.Skip(offset).Take(2).ToArray();
            if (arr.Length < 2)
            {
                throw new ArgumentOutOfRangeException(nameof(bytes), "Word needs at least 2 bytes.");
            }

            return (ushort) (arr[0] + arr[1]*0x100u);
        }

        public static uint ToDWord(this IEnumerable<byte> bytes, int offset = 0)
        {
            var arr = bytes.Skip(offset).Take(4).ToArray();
            if (arr.Length < 4)
            {
                throw new ArgumentOutOfRangeException(nameof(bytes), "DWord needs at least 4 bytes.");
            }

            return arr[0] + arr[1]*0x100u + arr[2]*0x10000u + arr[3]*0x1000000u;
        }

        public static char ToWChar(this IEnumerable<byte> bytes, int offset = 0)
        {
            var arr = bytes.Skip(offset).Take(2).ToArray();
            if (arr.Length < 2)
            {
                throw new ArgumentOutOfRangeException(nameof(bytes), "WChar needs at least 2 bytes.");
            }

            return (char) (arr[0] + arr[1]*0x100u);
        }

        public static HwpUnit ToHwpUnit(this IEnumerable<byte> bytes, int offset = 0)
        {
            var arr = bytes.Skip(offset).Take(4).ToArray();
            if (arr.Length < 4)
            {
                throw new ArgumentOutOfRangeException(nameof(bytes), "HwpUnit needs at least 4 bytes.");
            }

            return arr[0] + arr[1]*0x100u + arr[2]*0x10000u + arr[3]*0x1000000u;
        }

        public static SHwpUnit ToSHwpUnit(this IEnumerable<byte> bytes, int offset = 0)
        {
            var arr = bytes.Skip(offset).Take(4).ToArray();
            if (arr.Length < 4)
            {
                throw new ArgumentOutOfRangeException(nameof(bytes), "SHwpUnit needs at least 4 bytes.");
            }

            return arr[0] + arr[1]*0x100 + arr[2]*0x10000 + arr[3]*0x1000000;
        }

        public static ushort ToUInt16(this IEnumerable<byte> bytes, int offset = 0)
        {
            var arr = bytes.Skip(offset).Take(2).ToArray();
            if (arr.Length < 2)
            {
                throw new ArgumentOutOfRangeException(nameof(bytes), "UInt16 needs at least 2 bytes.");
            }

            return (ushort) (arr[0] + arr[1]*0x100u);
        }

        public static uint ToUInt32(this IEnumerable<byte> bytes, int offset = 0)
        {
            var arr = bytes.Skip(offset).Take(4).ToArray();
            if (arr.Length < 4)
            {
                throw new ArgumentOutOfRangeException(nameof(bytes), "UInt32 needs at least 4 bytes.");
            }

            return arr[0] + arr[1]*0x100u + arr[2]*0x10000u + arr[3]*0x1000000u;
        }

        public static short ToInt16(this IEnumerable<byte> bytes, int offset = 0)
        {
            var arr = bytes.Skip(offset).Take(2).ToArray();
            if (arr.Length < 2)
            {
                throw new ArgumentOutOfRangeException(nameof(bytes), "Int16 needs at least 2 bytes.");
            }

            return (short) (arr[0] + arr[1]*0x100);
        }

        public static int ToInt32(this IEnumerable<byte> bytes, int offset = 0)
        {
            var arr = bytes.Skip(offset).Take(4).ToArray();
            if (arr.Length < 4)
            {
                throw new ArgumentOutOfRangeException(nameof(bytes), "Int32 needs at least 4 bytes.");
            }

            return arr[0] + arr[1]*0x100 + arr[2]*0x10000 + arr[3]*0x1000000;
        }

        public static HwpUnit16 ToHwpUnit16(this IEnumerable<byte> bytes, int offset = 0)
        {
            var arr = bytes.Skip(offset).Take(2).ToArray();
            if (arr.Length < 2)
            {
                throw new ArgumentOutOfRangeException(nameof(bytes), "HwpUnit16 needs at least 2 bytes.");
            }

            return (short) (arr[0] + arr[1]*0x100);
        }

        public static Color ToColor(this IEnumerable<byte> bytes, int offset = 0)
        {
            var arr = bytes.Skip(offset).Take(4).ToArray();
            if (arr.Length < 4)
            {
                throw new ArgumentOutOfRangeException(nameof(bytes), "Color needs at least 4 bytes.");
            }

            return new Color(arr[0], arr[1], arr[2]);
        }
    }
}