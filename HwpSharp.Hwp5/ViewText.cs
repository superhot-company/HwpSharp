using System;
using System.Collections.Generic;
using System.Text;

namespace SuperHot.HwpSharp.Hwp5
{
    public class ViewText : IBodyText<ViewTextSection>
    {
        private readonly FileHeader _fileHeader;

        private readonly DocumentInformation _docInfo;

        public List<ViewTextSection> Sections { get; }

        public ViewText(FileHeader fileHeader, DocumentInformation docInfo)
        {
            _fileHeader = fileHeader;
            _docInfo = docInfo;
            Sections = new List<ViewTextSection>();
        }
    }
}
