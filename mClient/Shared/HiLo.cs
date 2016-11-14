using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.Shared
{
    public static class HiLo
    {
        /// <summary>
        /// Gets the Lo part of a UInt32
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ushort LoPart(this uint value)
        {
            var bytes = BitConverter.GetBytes(value);
            var loBytes = new byte[2];
            loBytes[0] = bytes[0];
            loBytes[1] = bytes[1];
            var lo = BitConverter.ToUInt16(loBytes, 0);
            return lo;
        }

        /// <summary>
        /// Gets the Hi part of a UInt32
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ushort HiPart(this uint value)
        {
            var bytes = BitConverter.GetBytes(value);
            var hiBytes = new byte[2];
            hiBytes[0] = bytes[2];
            hiBytes[1] = bytes[3];
            var hi = BitConverter.ToUInt16(hiBytes, 0);
            return hi;
        }
    }
}
