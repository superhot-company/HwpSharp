using System.Collections.Generic;
namespace SuperHot.HwpSharp.Hwp5
{
    /// <summary>
    /// Represents a hwp 5.0 body text.
    /// </summary>
    public partial class BodyText : IBodyText
    {
        private readonly FileHeader _fileHeader;

        private readonly DocumentInformation _docInfo;

        public List<Section> Sections { get; }

        public BodyText(FileHeader fileHeader, DocumentInformation docInfo)
        {
            _fileHeader = fileHeader;
            _docInfo = docInfo;
            Sections = new List<Section>();
        }
    }
}
