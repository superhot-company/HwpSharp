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
}