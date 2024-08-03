﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUIToolkit.Core
{
    //
    // Summary:
    //     Utility Class
    public class Utils
    {
        //
        // Summary:
        //     Bit-indexes for reversing.
        private static readonly byte[] DEF_REVERSE_BITS = new byte[16]
        {
        0, 8, 4, 12, 2, 10, 6, 14, 1, 9,
        5, 13, 3, 11, 7, 15
        };

        //
        // Summary:
        //     Code lengths for the code length alphabet.
        public static int[] DEF_HUFFMAN_DYNTREE_CODELENGTHS_ORDER = new int[19]
        {
        16, 17, 18, 0, 8, 7, 9, 6, 10, 5,
        11, 4, 12, 3, 13, 2, 14, 1, 15
        };

        //
        // Summary:
        //     Reverses bit.
        //
        // Parameters:
        //   value:
        public static short BitReverse(int value)
        {
            return (short)((DEF_REVERSE_BITS[value & 0xF] << 12) | (DEF_REVERSE_BITS[(value >> 4) & 0xF] << 8) | (DEF_REVERSE_BITS[(value >> 8) & 0xF] << 4) | DEF_REVERSE_BITS[value >> 12]);
        }
    }
}