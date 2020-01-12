using SuperHot.HwpSharp.Hwp5;
using System.Collections.Generic;

namespace SuperHot.HwpSharp
{
    /// <summary>
    /// Represents a body text.
    /// </summary>
    public interface IBodyText<T> where T : ISection
    {
        List<T> Sections { get; }
    }
}
