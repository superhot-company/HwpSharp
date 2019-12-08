using System.Collections.Generic;
using System.Text;

namespace SuperHot.HwpSharp.Hwp5.DataRecords
{
    public class ParagraphText : DataRecord
    {
        public const uint ParagraphTextTagId = HwpTagBegin + 51;

        public struct TextPartial
        {
            public static readonly ushort[] CharControl = { 0, 10, 13, 24, 25, 26, 27, 28, 29, 30, 31 };
            public static readonly ushort[] InlineControl = { 4, 5, 6, 7, 8, 9, 19, 20 };
            public static readonly ushort[] ExtendedControl = { 1, 2, 3, 11, 12, 14, 15, 16, 17, 18, 21, 22, 23 };

            public enum TextPartialType : ushort
            {
                Unknown = 0,

                Text = 1,
                CharControl = 2,
                InlineControl = 3,
                ExtendedControl = 4
            }

            public TextPartialType Type { get; set; }

            public string Text { get; set; }
        }

        public IList<TextPartial> Text { get; set; }

        public ParagraphText(uint level, byte[] bytes, FileHeader _ = null, DocumentInformation __ = null)
            : base(ParagraphTextTagId, level, (uint) bytes.Length, bytes)
        {
            Text = new List<TextPartial>();
            int index = 0, count = 0;
            for (int i = 0; i < bytes.Length; i += 2)
            {
                ushort cur = (ushort)(bytes[i] + bytes[i + 1] * 0x100u);

                if (System.Array.BinarySearch(TextPartial.CharControl, cur) >= 0)
                {
                    if (count > 0)
                    {
                        var str = Encoding.Unicode.GetString(bytes, index, count * 2);
                        Text.Add(new TextPartial { Type = TextPartial.TextPartialType.Text, Text = str });
                    }
                    index = i + 2; count = 0;

                    Text.Add(new TextPartial { Type = TextPartial.TextPartialType.CharControl, Text = char.ToString((char)cur) });
                }
                else if (System.Array.BinarySearch(TextPartial.InlineControl, cur) >= 0)
                {
                    if (count > 0)
                    {
                        var str = Encoding.Unicode.GetString(bytes, index, count * 2);
                        Text.Add(new TextPartial { Type = TextPartial.TextPartialType.Text, Text = str });
                    }
                    index = i + 16; count = 0;

                    var inlineStr = Encoding.Unicode.GetString(bytes, i, 16);
                    Text.Add(new TextPartial { Type = TextPartial.TextPartialType.InlineControl, Text = inlineStr });

                    i += 14;
                }
                else if (System.Array.BinarySearch(TextPartial.ExtendedControl, cur) >= 0)
                {
                    if (count > 0)
                    {
                        var str = Encoding.Unicode.GetString(bytes, index, count * 2);
                        Text.Add(new TextPartial { Type = TextPartial.TextPartialType.Text, Text = str });
                    }
                    index = i + 16; count = 0;

                    var extendedStr = Encoding.Unicode.GetString(bytes, i, 16);
                    Text.Add(new TextPartial { Type = TextPartial.TextPartialType.ExtendedControl, Text = extendedStr });

                    i += 14;
                }
                else
                {
                    ++count;
                }
            }
            if (count > 0)
            {
                var str = Encoding.Unicode.GetString(bytes, index, count * 2);
                Text.Add(new TextPartial { Type = TextPartial.TextPartialType.Text, Text = str });
            }
        }
    }
}