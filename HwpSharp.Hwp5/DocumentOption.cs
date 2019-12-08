using System.Collections.Generic;

namespace SuperHot.HwpSharp.Hwp5
{
    public class DocumentOption
    {
        public IDictionary<string, byte[]> Streams { get; set; }

        public DocumentOption()
        {
            Streams = new Dictionary<string, byte[]>();
        }
    }
}