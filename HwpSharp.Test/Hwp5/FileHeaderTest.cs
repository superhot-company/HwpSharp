using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SuperHot.HwpSharp.Common;
using SuperHot.HwpSharp.Hwp5;
using Xunit;

namespace SuperHot.HwpSharp.Test.Hwp5
{
    public class FileHeaderTest
    {
        [Theory]
        [InlineData(@"../../../case/Hwp5/BlogForm_BookReview.hwp", "5.0.3.0")]
        [InlineData(@"../../../case/Hwp5/BlogForm_MovieReview.hwp", "5.0.3.0")]
        [InlineData(@"../../../case/Hwp5/BlogForm_Recipe.hwp", "5.0.3.0")]
        [InlineData(@"../../../case/Hwp5/BookReview.hwp", "5.0.3.0")]
        [InlineData(@"../../../case/Hwp5/calendar_monthly.hwp", "5.0.3.0")]
        [InlineData(@"../../../case/Hwp5/calendar_year.hwp", "5.0.3.0")]
        [InlineData(@"../../../case/Hwp5/classical_literature.hwp", "5.0.3.0")]
        [InlineData(@"../../../case/Hwp5/english.hwp", "5.0.3.2")]
        [InlineData(@"../../../case/Hwp5/Hyper(hwp2010).hwp", "5.0.3.3")]
        [InlineData(@"../../../case/Hwp5/interview.hwp", "5.0.3.0")]
        [InlineData(@"../../../case/Hwp5/KTX.hwp", "5.0.3.0")]
        [InlineData(@"../../../case/Hwp5/NewYear_s_Day.hwp", "5.0.3.2")]
        [InlineData(@"../../../case/Hwp5/request.hwp", "5.0.3.2")]
        [InlineData(@"../../../case/Hwp5/shortcut.hwp", "5.0.3.0")]
        [InlineData(@"../../../case/Hwp5/sungeo.hwp", "5.0.3.0")]
        [InlineData(@"../../../case/Hwp5/Textmail.hwp", "5.0.3.0")]
        [InlineData(@"../../../case/Hwp5/treatise sample.hwp", "5.0.3.0")]
        [InlineData(@"../../../case/Hwp5/Worldcup_FIFA2010_32.hwp", "5.0.3.0")]
        public void Version_NormalHwp5Document(string filename, string expected)
        {
            var document = new Document(filename);

            Assert.Equal(expected, document.FileHeader.FileVersion.ToString());
        }

        [Theory]
        [InlineData(@"../../../case/Hwp5/ccl_trackchange.hwp", true, false, false, false, false, false, false, false, false, false, false, true, false, false, true, false, false, false)]
        [InlineData(@"../../../case/Hwp5/distribution.hwp", true, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false)]
        public void Attribute_Hwp5Dcoument(string filename, bool compressed, bool encrypted, bool distribute, bool script, bool drm, bool xml, bool history, bool sign, bool certificate, bool reservedCertificate, bool certificateDrm, bool ccl, bool mobileOptimized, bool personalinformation, bool trackchange, bool kogl, bool video, bool index)
        {
            var document = new Document(filename);

            Assert.Equal(compressed, document.FileHeader.Compressed);
            Assert.Equal(encrypted, document.FileHeader.PasswordEncrypted);
            Assert.Equal(distribute, document.FileHeader.Published);
            Assert.Equal(ccl, document.FileHeader.CclDocumented);
            Assert.Equal(trackchange, document.FileHeader.TrackChange);
        }
    }
}
