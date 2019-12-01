using SuperHot.HwpSharp.Common.HwpType;
using System.Collections;
using System.Collections.Generic;

namespace SuperHot.HwpSharp.Common
{
    public class Random : IEnumerable<uint>
    {
        public uint Seed { get; private set; }

        public Random(uint seed)
        {
            Seed = seed;
        }

        public IEnumerator<uint> GetEnumerator()
        {
            while (true)
            {
                Seed = (Seed*214013 + 2531011) & 0xFFFFFFFF;
                yield return (Seed >> 16) & 0x7FFF;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
