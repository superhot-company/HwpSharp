using System.Collections.Generic;

namespace SuperHot.HwpSharp.Hwp5
{
    public class XmlTemplate
    {
        // Schema
        // Instance
        // ...
        public IDictionary<string, byte[]> Streams { get; set; }

        public XmlTemplate()
        {
            Streams = new Dictionary<string, byte[]>();
        }
    }
}