using SuperHot.HwpSharp.Hwp5;
using Xunit;

namespace SuperHot.HwpSharp.Test.Hwp5
{
    public class DistributionTest
    {
        [Theory]
        [InlineData(@"../../../case/Hwp5/[__]announce.hwp", "5.0.2.4")]
        [InlineData(@"../../../case/Hwp5/[_P]psat.hwp", "5.0.2.4")]
        [InlineData(@"../../../case/Hwp5/[_P]psat2.hwp", "5.0.3.4")]
        [InlineData(@"../../../case/Hwp5/[CP]manual.hwp", "5.0.5.0")]
        public void ReadDistributionFile(string filename, string version)
        {
            var document = new Document(filename);
            Assert.Equal(version, document.FileHeader.FileVersion.ToString());
            Assert.Equal(true, document.FileHeader.Distributed);
        }
    }
}
