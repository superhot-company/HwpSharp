﻿using System.Linq;
using SuperHot.HwpSharp.Hwp5;
using SuperHot.HwpSharp.Hwp5.DataRecords;
using Xunit;

namespace SuperHot.HwpSharp.Test.Hwp5
{
    public class BodyTextTest
    {
        [Theory]
        // [InlineData(@"../../../case/Hwp5/BlogForm_BookReview.hwp", null)]
        // [InlineData(@"../../../case/Hwp5/BlogForm_MovieReview.hwp", null)]
        // [InlineData(@"../../../case/Hwp5/BlogForm_Recipe.hwp", null)]
        // [InlineData(@"../../../case/Hwp5/BookReview.hwp", null)]
        // [InlineData(@"../../../case/Hwp5/calendar_monthly.hwp", null)]
        // [InlineData(@"../../../case/Hwp5/calendar_year.hwp", null)]
        // [InlineData(@"../../../case/Hwp5/classical_literature.hwp", null)]
        // [InlineData(@"../../../case/Hwp5/english.hwp", null)]
        // [InlineData(@"../../../case/Hwp5/Hyper(hwp2010).hwp", null)]
        // [InlineData(@"../../../case/Hwp5/interview.hwp", null)]
        // [InlineData(@"../../../case/Hwp5/KTX.hwp", null)]
        // [InlineData(@"../../../case/Hwp5/NewYear_s_Day.hwp", null)]
        // [InlineData(@"../../../case/Hwp5/request.hwp", null)]
        // [InlineData(@"../../../case/Hwp5/shortcut.hwp", null)]
        // [InlineData(@"../../../case/Hwp5/sungeo.hwp", null)]
        // [InlineData(@"../../../case/Hwp5/Textmail.hwp", null)]
        // [InlineData(@"../../../case/Hwp5/treatise sample.hwp", null)]
        // [InlineData(@"../../../case/Hwp5/Worldcup_FIFA2010_32.hwp", null)]
        [InlineData(@"../../../case/Hwp5/multisection.hwp", "\u0002\u6364\u7365\0\0\0\0\u0002\u0002\u6c64\u636f\0\0\0\0\u0002\u0015\u6e70\u7067\0\0\0\0\u0015MultiSection 01\r")]
        public void ParagraphText_NormalHwp5Document(string filename, string expectedBodyText)
        {
            var document = new Document(filename);

            var paragraph =
                document.BodyText.Sections[0].DataRecords.Where(
                    record => record.TagId == (uint)TagEnum.ParagraphText)
                    .Cast<ParagraphText>()
                    .Aggregate("", (x, y) => x + y?.Text.Aggregate("", (a, b) => a + b.Text));
            Assert.Equal(expectedBodyText, paragraph);
        }
    }
}
