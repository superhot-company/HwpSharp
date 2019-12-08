using System.Collections.Generic;

namespace SuperHot.HwpSharp.Hwp5
{
    public class Script
    {
        // DefaultJScript
        // JScriptVersion
        // ...
        public IDictionary<string, byte[]> Streams { get; set; }

        public Script()
        {
            Streams = new Dictionary<string, byte[]>();
        }
    }
}