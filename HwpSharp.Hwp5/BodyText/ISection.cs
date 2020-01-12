using SuperHot.HwpSharp.Hwp5.DataRecords;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuperHot.HwpSharp.Hwp5
{
    public interface ISection
    {
        List<DataRecord> DataRecords { get; }
    }
}
