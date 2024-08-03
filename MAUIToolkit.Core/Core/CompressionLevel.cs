using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUIToolkit.Core
{
    //
    // Summary:
    //     Compression level.
    public enum CompressionLevel
    {
        //
        // Summary:
        //     Pack without compression
        NoCompression = 0,
        //
        // Summary:
        //     Use high speed compression, reduce of data size is low
        BestSpeed = 1,
        //
        // Summary:
        //     Something middle between normal and BestSpeed compressions
        BelowNormal = 3,
        //
        // Summary:
        //     Use normal compression, middle between speed and size
        Normal = 5,
        //
        // Summary:
        //     Pack better but require a little more time
        AboveNormal = 7,
        //
        // Summary:
        //     Use best compression, slow enough
        Best = 9
    }
}
