using SuperHot.HwpSharp.Hwp5;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace SuperHot.HwpSharp.Test.Hwp5
{
    public class BinDataTest
    {
        [Theory]
        [InlineData(@"../../../case/Hwp5/BlogForm_BookReview.hwp", 1)]
        [InlineData(@"../../../case/Hwp5/treatise sample.hwp", 6)]
        [InlineData(@"../../../case/Hwp5/request.hwp", 1)]
        [InlineData(@"../../../case/Hwp5/image.hwp", 1)]
        public void ReadDistributionFile(string filename, int count)
        {
            var document = new Document(filename);
            Assert.Equal(count, document.BinaryData.Data.Count);
        }
    }
}
