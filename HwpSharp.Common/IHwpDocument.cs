namespace SuperHot.HwpSharp.Common
{
    /// <summary>
    /// Represents a Hwp document.
    /// </summary>
    public interface IHwpDocument
    {
        /// <summary>
        /// Represents the version of Hwp document.
        /// </summary>
        string HwpVersion { get; }
    }
}
