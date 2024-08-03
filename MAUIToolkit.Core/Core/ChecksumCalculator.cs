using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUIToolkit.Core
{
    //
    // Summary:
    //     Checksum calculator, based on Adler32 algorithm.
    public class ChecksumCalculator
    {
        //
        // Summary:
        //     Bits offset, used in adler checksum calculation.
        private const int DEF_CHECKSUM_BIT_OFFSET = 16;

        //
        // Summary:
        //     Lagrest prime, less than 65535
        private const int DEF_CHECKSUM_BASE = 65521;

        //
        // Summary:
        //     Count of iteration used in calculated of the adler checksumm.
        private const int DEF_CHECKSUM_ITERATIONSCOUNT = 3800;

        //
        // Summary:
        //     Updates checksum by calculating checksum of the given buffer and adding it to
        //     current value.
        //
        // Parameters:
        //   checksum:
        //     Current checksum.
        //
        //   buffer:
        //     Data byte array.
        //
        //   offset:
        //     Offset in the buffer.
        //
        //   length:
        //     Length of data to be used from the stream.
        public static void ChecksumUpdate(ref long checksum, byte[] buffer, int offset, int length)
        {
            uint num = (uint)checksum;
            uint num2 = num & 0xFFFFu;
            uint num3 = num >> 16;
            while (length > 0)
            {
                int num4 = Math.Min(length, 3800);
                length -= num4;
                while (--num4 >= 0)
                {
                    num2 += (uint)(buffer[offset++] & 0xFF);
                    num3 += num2;
                }

                num2 %= 65521;
                num3 %= 65521;
            }

            num = (num3 << 16) | num2;
            checksum = num;
        }

        //
        // Summary:
        //     Generates checksum by calculating checksum of the given buffer.
        //
        // Parameters:
        //   buffer:
        //     Data byte array.
        //
        //   offset:
        //     Offset in the buffer.
        //
        //   length:
        //     Length of data to be used from the stream.
        public static long ChecksumGenerate(byte[] buffer, int offset, int length)
        {
            long checksum = 1L;
            ChecksumUpdate(ref checksum, buffer, offset, length);
            return checksum;
        }
    }
}
