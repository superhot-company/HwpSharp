using System.Collections.Generic;

namespace SuperHot.HwpSharp.Hwp5
{
    public class DocumentHistory
    {
        // HistoryLastDoc
        // VersionLog0
        // VersionLog1
        // ...
        public IDictionary<string, byte[]> Streams { get; set; }

        public DocumentHistory()
        {
            Streams = new Dictionary<string, byte[]>();
        }
    }
}