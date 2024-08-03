using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUIToolkit.Core
{
    //
    // Summary:
    //     Represents the compressed stream writer
    public class CompressedStreamWriter
    {
        //
        // Summary:
        //     Type of the block.
        private enum BlockType
        {
            //
            // Summary:
            //     Data simply stored as is
            Stored,
            //
            // Summary:
            //     An option to use Fixed Huffman tree codes
            FixedHuffmanCodes,
            //
            // Summary:
            //     An option to use Dynamically built Huffman codes
            DynamicHuffmanCodes
        }

        //
        // Summary:
        //     Start template of the zlib header.
        private const int DEF_ZLIB_HEADER_TEMPLATE = 30720;

        //
        // Summary:
        //     Memory usage level.
        private const int DEFAULT_MEM_LEVEL = 8;

        //
        // Summary:
        //     Size of the pending buffer.
        private const int DEF_PENDING_BUFFER_SIZE = 65536;

        //
        // Summary:
        //     Size of the buffer for the huffman encoding.
        private const int DEF_HUFFMAN_BUFFER_SIZE = 16384;

        //
        // Summary:
        //     Length of the literal alphabet(literal+lengths).
        private const int DEF_HUFFMAN_LITERAL_ALPHABET_LENGTH = 286;

        //
        // Summary:
        //     Distances alphabet length.
        private const int DEF_HUFFMAN_DISTANCES_ALPHABET_LENGTH = 30;

        //
        // Summary:
        //     Length of the code-lengths tree.
        private const int DEF_HUFFMAN_BITLEN_TREE_LENGTH = 19;

        //
        // Summary:
        //     Code of the symbol, than means the end of the block.
        private const int DEF_HUFFMAN_ENDBLOCK_SYMBOL = 256;

        private const int TOO_FAR = 4096;

        //
        // Summary:
        //     Maximum window size.
        private const int WSIZE = 32768;

        //
        // Summary:
        //     Internal compression engine constant
        public const int WMASK = 32767;

        //
        // Summary:
        //     Internal compression engine constant
        public const int HASH_BITS = 15;

        //
        // Summary:
        //     Internal compression engine constant
        public const int HASH_SIZE = 32768;

        //
        // Summary:
        //     Internal compression engine constant
        public const int HASH_MASK = 32767;

        //
        // Summary:
        //     Internal compression engine constant
        public const int MAX_MATCH = 258;

        //
        // Summary:
        //     Internal compression engine constant
        public const int MIN_MATCH = 3;

        //
        // Summary:
        //     Internal compression engine constant
        public const int HASH_SHIFT = 5;

        //
        // Summary:
        //     Internal compression engine constant
        public const int MIN_LOOKAHEAD = 262;

        //
        // Summary:
        //     Internal compression engine constant
        public const int MAX_DIST = 32506;

        //
        // Summary:
        //     Internal compression engine constant
        public static int[] GOOD_LENGTH;

        //
        // Summary:
        //     Internal compression engine constant
        public static int[] MAX_LAZY;

        //
        // Summary:
        //     Internal compression engine constant
        public static int[] NICE_LENGTH;

        //
        // Summary:
        //     Internal compression engine constant
        public static int[] MAX_CHAIN;

        //
        // Summary:
        //     Internal compression engine constant
        public static int[] COMPR_FUNC;

        //
        // Summary:
        //     Internal compression engine constant
        public static int MAX_BLOCK_SIZE;

        //
        // Summary:
        //     Output stream.
        private Stream m_stream;

        //
        // Summary:
        //     Pending buffer for writing.
        private byte[] m_PendingBuffer = new byte[65536];

        //
        // Summary:
        //     Length of the unflushed data.
        private int m_PendingBufferLength;

        //
        // Summary:
        //     Bits cache for pending buffer.
        private uint m_PendingBufferBitsCache;

        //
        // Summary:
        //     Count of bits in pending buffer cache.
        private int m_PendingBufferBitsInCache;

        //
        // Summary:
        //     If true, no zlib header will be written to the stream.
        private bool m_bNoWrap;

        //
        // Summary:
        //     Current checksum.
        private long m_CheckSum = 1L;

        //
        // Summary:
        //     Current compression level.
        private CompressionLevel m_Level;

        //
        // Summary:
        //     Current tree for literals.
        private CompressorHuffmanTree m_treeLiteral;

        //
        // Summary:
        //     Current tree for distances.
        private CompressorHuffmanTree m_treeDistances;

        //
        // Summary:
        //     Current tree for code lengths.
        private CompressorHuffmanTree m_treeCodeLengths;

        //
        // Summary:
        //     Current position in literals and distances buffer.
        private int m_iBufferPosition;

        //
        // Summary:
        //     Recorded literals buffer.
        private byte[] m_arrLiteralsBuffer;

        //
        // Summary:
        //     Recorded distances buffer.
        private short[] m_arrDistancesBuffer;

        //
        // Summary:
        //     Count of the extra bits.
        private int m_iExtraBits;

        //
        // Summary:
        //     Static array of the literal codes.
        private static short[] m_arrLiteralCodes;

        //
        // Summary:
        //     Static array of the lengths of the literal codes.
        private static byte[] m_arrLiteralLengths;

        //
        // Summary:
        //     Static array of the distance codes.
        private static short[] m_arrDistanceCodes;

        //
        // Summary:
        //     Static array of the lengths of the distance codes.
        private static byte[] m_arrDistanceLengths;

        //
        // Summary:
        //     If true, no futher writings can be performed.
        private bool m_bStreamClosed;

        //
        // Summary:
        //     Current hash.
        private int m_CurrentHash;

        //
        // Summary:
        //     Hash m_HashHead.
        private short[] m_HashHead;

        //
        // Summary:
        //     Previous hashes.
        private short[] m_HashPrevious;

        //
        // Summary:
        //     Start of the matched part.
        private int m_MatchStart;

        //
        // Summary:
        //     Length of the matched part.
        private int m_MatchLength;

        //
        // Summary:
        //     Previous match available.
        private bool m_MatchPreviousAvailable;

        //
        // Summary:
        //     Start of the data window.
        private int m_BlockStart;

        //
        // Summary:
        //     String start in data window.
        private int m_StringStart;

        //
        // Summary:
        //     Lookahead.
        private int m_LookAhead;

        //
        // Summary:
        //     Data window.
        private byte[] m_DataWindow;

        //
        // Summary:
        //     Maximum chain length.
        private int m_MaximumChainLength;

        //
        // Summary:
        //     Maximum distance of the search with "lazy" algotithm.
        private int m_MaximumLazySearch;

        //
        // Summary:
        //     Nice length of the block.
        private int m_NiceLength;

        //
        // Summary:
        //     Good length of the block.
        private int m_GoodLength;

        //
        // Summary:
        //     Current compression function.
        private int m_CompressionFunction;

        //
        // Summary:
        //     Current block of the data to be compressed.
        private byte[] m_InputBuffer;

        //
        // Summary:
        //     Total count of bytes, that were compressed.
        private int m_TotalBytesIn;

        //
        // Summary:
        //     Offset in the input buffer, where input starts.
        private int m_InputOffset;

        //
        // Summary:
        //     Offset in the input buffer, where input ends.
        private int m_InputEnd;

        //
        // Summary:
        //     If true, stream will be closed after the last block.
        private bool m_bCloseStream;

        //
        // Summary:
        //     Total data processed.
        public int TotalIn => m_TotalBytesIn;

        //
        // Summary:
        //     Return true if input is needed
        private bool NeedsInput => m_InputEnd == m_InputOffset;

        //
        // Summary:
        //     Checks, wheather huffman compression buffer is full.
        //
        // Returns:
        //     True if buffer is full.
        private bool HuffmanIsFull => m_iBufferPosition >= 16384;

        //
        // Summary:
        //     The number of bits written to the buffer
        internal int PendingBufferBitCount => m_PendingBufferBitsInCache;

        //
        // Summary:
        //     Indicates if buffer has been flushed
        internal bool PendingBufferIsFlushed => m_PendingBufferLength == 0;

        //
        // Summary:
        //     Initializes statical data for huffman compression.
        static CompressedStreamWriter()
        {
            GOOD_LENGTH = new int[10] { 0, 4, 4, 4, 4, 8, 8, 8, 32, 32 };
            MAX_LAZY = new int[10] { 0, 4, 5, 6, 4, 16, 16, 32, 128, 258 };
            NICE_LENGTH = new int[10] { 0, 8, 16, 32, 16, 32, 128, 128, 258, 258 };
            MAX_CHAIN = new int[10] { 0, 4, 8, 32, 16, 32, 128, 256, 1024, 4096 };
            COMPR_FUNC = new int[10] { 0, 1, 1, 1, 1, 2, 2, 2, 2, 2 };
            MAX_BLOCK_SIZE = Math.Min(65535, 65531);
            m_arrLiteralCodes = new short[286];
            m_arrLiteralLengths = new byte[286];
            int num = 0;
            while (num < 144)
            {
                m_arrLiteralCodes[num] = Utils.BitReverse(48 + num << 8);
                m_arrLiteralLengths[num++] = 8;
            }

            while (num < 256)
            {
                m_arrLiteralCodes[num] = Utils.BitReverse(256 + num << 7);
                m_arrLiteralLengths[num++] = 9;
            }

            while (num < 280)
            {
                m_arrLiteralCodes[num] = Utils.BitReverse(-256 + num << 9);
                m_arrLiteralLengths[num++] = 7;
            }

            while (num < 286)
            {
                m_arrLiteralCodes[num] = Utils.BitReverse(-88 + num << 8);
                m_arrLiteralLengths[num++] = 8;
            }

            m_arrDistanceCodes = new short[30];
            m_arrDistanceLengths = new byte[30];
            for (num = 0; num < 30; num++)
            {
                m_arrDistanceCodes[num] = Utils.BitReverse(num << 11);
                m_arrDistanceLengths[num] = 5;
            }
        }

        //
        // Summary:
        //     Initializes compressor and writes ZLib header if needed.
        //
        // Parameters:
        //   outputStream:
        //     Output stream.
        //
        //   bNoWrap:
        //     If true, ZLib header and checksum will not be written.
        //
        //   level:
        //     Compression level.
        //
        //   bCloseStream:
        //     If true, output stream will be closed after the last block has been written.
        public CompressedStreamWriter(Stream outputStream, bool bNoWrap, CompressionLevel level, bool bCloseStream)
        {
            if (outputStream == null)
            {
                throw new ArgumentNullException("outputStream");
            }

            if (!outputStream.CanWrite)
            {
                throw new ArgumentException("Output stream does not support writing.", "outputStream");
            }

            m_treeLiteral = new CompressorHuffmanTree(this, 286, 257, 15);
            m_treeDistances = new CompressorHuffmanTree(this, 30, 1, 15);
            m_treeCodeLengths = new CompressorHuffmanTree(this, 19, 4, 7);
            m_arrDistancesBuffer = new short[16384];
            m_arrLiteralsBuffer = new byte[16384];
            m_stream = outputStream;
            m_Level = level;
            m_bNoWrap = bNoWrap;
            m_bCloseStream = bCloseStream;
            m_DataWindow = new byte[65536];
            m_HashHead = new short[32768];
            m_HashPrevious = new short[32768];
            m_BlockStart = (m_StringStart = 1);
            m_GoodLength = GOOD_LENGTH[(int)level];
            m_MaximumLazySearch = MAX_LAZY[(int)level];
            m_NiceLength = NICE_LENGTH[(int)level];
            m_MaximumChainLength = MAX_CHAIN[(int)level];
            m_CompressionFunction = COMPR_FUNC[(int)level];
            if (!bNoWrap)
            {
                WriteZLIBHeader();
            }
        }

        //
        // Summary:
        //     Initializes compressor and writes ZLib header if needed. Compression level is
        //     set to normal.
        //
        // Parameters:
        //   outputStream:
        //     Output stream.
        //
        //   bNoWrap:
        //     If true, ZLib header and checksum will not be written.
        //
        //   bCloseStream:
        //     If true, output stream will be closed after the last block has been written.
        public CompressedStreamWriter(Stream outputStream, bool bNoWrap, bool bCloseStream)
            : this(outputStream, bNoWrap, CompressionLevel.Normal, bCloseStream)
        {
        }

        //
        // Summary:
        //     Initializes compressor and writes ZLib header.
        //
        // Parameters:
        //   outputStream:
        //     Output stream.
        //
        //   level:
        //     Compression level.
        //
        //   bCloseStream:
        //     If true, output stream will be closed after the last block has been written.
        public CompressedStreamWriter(Stream outputStream, CompressionLevel level, bool bCloseStream)
            : this(outputStream, bNoWrap: false, level, bCloseStream)
        {
        }

        //
        // Summary:
        //     Initializes compressor and writes ZLib header.
        //
        // Parameters:
        //   outputStream:
        //     Output stream.
        //
        //   bCloseStream:
        //     If true, output stream will be closed after the last block has been written.
        public CompressedStreamWriter(Stream outputStream, bool bCloseStream)
            : this(outputStream, bNoWrap: false, CompressionLevel.Normal, bCloseStream)
        {
        }

        //
        // Summary:
        //     Compresses data and writes it to the stream.
        //
        // Parameters:
        //   data:
        //     Data to compress
        //
        //   offset:
        //     offset in data array
        //
        //   length:
        //     length of data to compress
        //
        //   bCloseAfterWrite:
        //     True - write last compress block in stream, otherwise False
        public void Write(byte[] data, int offset, int length, bool bCloseAfterWrite)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            int num = offset + length;
            if (0 > offset || offset > num || num > data.Length)
            {
                throw new ArgumentOutOfRangeException("Offset or length is incorrect.");
            }

            m_InputBuffer = data;
            m_InputOffset = offset;
            m_InputEnd = num;
            if (length == 0)
            {
                return;
            }

            if (m_bStreamClosed)
            {
                throw new IOException("Stream was closed.");
            }

            ChecksumCalculator.ChecksumUpdate(ref m_CheckSum, m_InputBuffer, m_InputOffset, length);
            while (!NeedsInput || !PendingBufferIsFlushed)
            {
                PendingBufferFlush();
                if (!CompressData(bCloseAfterWrite) && bCloseAfterWrite)
                {
                    PendingBufferFlush();
                    PendingBufferAlignToByte();
                    if (!m_bNoWrap)
                    {
                        PendingBufferWriteShortMSB((int)(m_CheckSum >> 16));
                        PendingBufferWriteShortMSB((int)(m_CheckSum & 0xFFFF));
                    }

                    PendingBufferFlush();
                    m_bStreamClosed = true;
                    if (m_bCloseStream)
                    {
                        m_stream.Close();
                    }
                }
            }
        }

        public void Close()
        {
            if (m_bStreamClosed)
            {
                return;
            }

            do
            {
                PendingBufferFlush();
                if (!CompressData(finish: true))
                {
                    PendingBufferFlush();
                    PendingBufferAlignToByte();
                    if (!m_bNoWrap)
                    {
                        PendingBufferWriteShortMSB((int)(m_CheckSum >> 16));
                        PendingBufferWriteShortMSB((int)(m_CheckSum & 0xFFFF));
                    }

                    PendingBufferFlush();
                }
            }
            while (!NeedsInput || !PendingBufferIsFlushed);
            m_bStreamClosed = true;
            if (m_bCloseStream)
            {
                m_stream.Close();
            }
        }

        //
        // Summary:
        //     Writes ZLib header to stream.
        private void WriteZLIBHeader()
        {
            int num = 30720;
            num |= (((int)m_Level >> 2) & 3) << 6;
            num += 31 - num % 31;
            PendingBufferWriteShortMSB(num);
        }

        //
        // Summary:
        //     Fill the window
        private void FillWindow()
        {
            if (m_StringStart >= 65274)
            {
                SlideWindow();
            }

            while (m_LookAhead < 262 && m_InputOffset < m_InputEnd)
            {
                int num = 65536 - m_LookAhead - m_StringStart;
                if (num > m_InputEnd - m_InputOffset)
                {
                    num = m_InputEnd - m_InputOffset;
                }

                Array.Copy(m_InputBuffer, m_InputOffset, m_DataWindow, m_StringStart + m_LookAhead, num);
                m_InputOffset += num;
                m_TotalBytesIn += num;
                m_LookAhead += num;
            }

            if (m_LookAhead >= 3)
            {
                UpdateHash();
            }
        }

        //
        // Summary:
        //     Slides current window, and data, associated with it.
        private void SlideWindow()
        {
            Array.Copy(m_DataWindow, 32768, m_DataWindow, 0, 32768);
            m_MatchStart -= 32768;
            m_StringStart -= 32768;
            m_BlockStart -= 32768;
            for (int i = 0; i < 32768; i++)
            {
                int num = m_HashHead[i] & 0xFFFF;
                m_HashHead[i] = (short)((num >= 32768) ? (num - 32768) : 0);
            }

            for (int j = 0; j < 32768; j++)
            {
                int num2 = m_HashPrevious[j] & 0xFFFF;
                m_HashPrevious[j] = (short)((num2 >= 32768) ? (num2 - 32768) : 0);
            }
        }

        //
        // Summary:
        //     Updates hash.
        private void UpdateHash()
        {
            m_CurrentHash = (m_DataWindow[m_StringStart] << 5) ^ m_DataWindow[m_StringStart + 1];
        }

        //
        // Summary:
        //     Inserts string to the hash.
        private int InsertString()
        {
            int num = ((m_CurrentHash << 5) ^ m_DataWindow[m_StringStart + 2]) & 0x7FFF;
            short num2 = (m_HashPrevious[m_StringStart & 0x7FFF] = m_HashHead[num]);
            m_HashHead[num] = (short)m_StringStart;
            m_CurrentHash = num;
            return num2 & 0xFFFF;
        }

        //
        // Summary:
        //     Searches for the longest match.
        //
        // Parameters:
        //   curMatch:
        private bool FindLongestMatch(int curMatch)
        {
            int num = m_MaximumChainLength;
            int num2 = m_NiceLength;
            short[] hashPrevious = m_HashPrevious;
            int stringStart = m_StringStart;
            int num3 = m_StringStart + m_MatchLength;
            int num4 = Math.Max(m_MatchLength, 2);
            int num5 = Math.Max(m_StringStart - 32506, 0);
            int num6 = m_StringStart + 258 - 1;
            byte b = m_DataWindow[num3 - 1];
            byte b2 = m_DataWindow[num3];
            if (num4 >= m_GoodLength)
            {
                num >>= 2;
            }

            if (num2 > m_LookAhead)
            {
                num2 = m_LookAhead;
            }

            do
            {
                if (m_DataWindow[curMatch + num4] != b2 || m_DataWindow[curMatch + num4 - 1] != b || m_DataWindow[curMatch] != m_DataWindow[stringStart] || m_DataWindow[curMatch + 1] != m_DataWindow[stringStart + 1])
                {
                    continue;
                }

                int num7 = curMatch + 2;
                stringStart += 2;
                while (m_DataWindow[++stringStart] == m_DataWindow[++num7] && m_DataWindow[++stringStart] == m_DataWindow[++num7] && m_DataWindow[++stringStart] == m_DataWindow[++num7] && m_DataWindow[++stringStart] == m_DataWindow[++num7] && m_DataWindow[++stringStart] == m_DataWindow[++num7] && m_DataWindow[++stringStart] == m_DataWindow[++num7] && m_DataWindow[++stringStart] == m_DataWindow[++num7] && m_DataWindow[++stringStart] == m_DataWindow[++num7] && stringStart < num6)
                {
                }

                if (stringStart > num3)
                {
                    m_MatchStart = curMatch;
                    num3 = stringStart;
                    num4 = stringStart - m_StringStart;
                    if (num4 >= num2)
                    {
                        break;
                    }

                    b = m_DataWindow[num3 - 1];
                    b2 = m_DataWindow[num3];
                }

                stringStart = m_StringStart;
            }
            while ((curMatch = hashPrevious[curMatch & 0x7FFF] & 0xFFFF) > num5 && --num != 0);
            m_MatchLength = Math.Min(num4, m_LookAhead);
            return m_MatchLength >= 3;
        }

        //
        // Summary:
        //     Store data without compression.
        //
        // Parameters:
        //   flush:
        //
        //   finish:
        private bool SaveStored(bool flush, bool finish)
        {
            if (!flush && m_LookAhead == 0)
            {
                return false;
            }

            m_StringStart += m_LookAhead;
            m_LookAhead = 0;
            int num = m_StringStart - m_BlockStart;
            if (num >= MAX_BLOCK_SIZE || (m_BlockStart < 32768 && num >= 32506) || flush)
            {
                bool flag = finish;
                if (num > MAX_BLOCK_SIZE)
                {
                    num = MAX_BLOCK_SIZE;
                    flag = false;
                }

                HuffmanFlushStoredBlock(m_DataWindow, m_BlockStart, num, flag);
                m_BlockStart += num;
                return !flag;
            }

            return true;
        }

        //
        // Summary:
        //     Compress with a maximum speed.
        //
        // Parameters:
        //   flush:
        //
        //   finish:
        private bool CompressFast(bool flush, bool finish)
        {
            if (m_LookAhead < 262 && !flush)
            {
                return false;
            }

            while (m_LookAhead >= 262 || flush)
            {
                if (m_LookAhead == 0)
                {
                    HuffmanFlushBlock(m_DataWindow, m_BlockStart, m_StringStart - m_BlockStart, finish);
                    m_BlockStart = m_StringStart;
                    return false;
                }

                if (m_StringStart > 65274)
                {
                    SlideWindow();
                }

                int num;
                if (m_LookAhead >= 3 && (num = InsertString()) != 0 && m_StringStart - num <= 32506 && FindLongestMatch(num))
                {
                    if (HuffmanTallyDist(m_StringStart - m_MatchStart, m_MatchLength))
                    {
                        bool lastBlock = finish && m_LookAhead == 0;
                        HuffmanFlushBlock(m_DataWindow, m_BlockStart, m_StringStart - m_BlockStart, lastBlock);
                        m_BlockStart = m_StringStart;
                    }

                    m_LookAhead -= m_MatchLength;
                    if (m_MatchLength <= m_MaximumLazySearch && m_LookAhead >= 3)
                    {
                        while (--m_MatchLength > 0)
                        {
                            m_StringStart++;
                            InsertString();
                        }

                        m_StringStart++;
                    }
                    else
                    {
                        m_StringStart += m_MatchLength;
                        if (m_LookAhead >= 2)
                        {
                            UpdateHash();
                        }
                    }

                    m_MatchLength = 2;
                }
                else
                {
                    HuffmanTallyLit(m_DataWindow[m_StringStart] & 0xFF);
                    m_StringStart++;
                    m_LookAhead--;
                    if (HuffmanIsFull)
                    {
                        bool flag = finish && m_LookAhead == 0;
                        HuffmanFlushBlock(m_DataWindow, m_BlockStart, m_StringStart - m_BlockStart, flag);
                        m_BlockStart = m_StringStart;
                        return !flag;
                    }
                }
            }

            return true;
        }

        //
        // Summary:
        //     Compress, using maximum compression level.
        //
        // Parameters:
        //   flush:
        //
        //   finish:
        private bool CompressSlow(bool flush, bool finish)
        {
            if (m_LookAhead < 262 && !flush)
            {
                return false;
            }

            while (m_LookAhead >= 262 || flush)
            {
                if (m_LookAhead == 0)
                {
                    if (m_MatchPreviousAvailable)
                    {
                        HuffmanTallyLit(m_DataWindow[m_StringStart - 1] & 0xFF);
                    }

                    m_MatchPreviousAvailable = false;
                    HuffmanFlushBlock(m_DataWindow, m_BlockStart, m_StringStart - m_BlockStart, finish);
                    m_BlockStart = m_StringStart;
                    return false;
                }

                if (m_StringStart >= 65274)
                {
                    SlideWindow();
                }

                int matchStart = m_MatchStart;
                int matchLength = m_MatchLength;
                if (m_LookAhead >= 3)
                {
                    int num = InsertString();
                    if (num != 0 && m_StringStart - num <= 32506 && FindLongestMatch(num) && m_MatchLength <= 5 && m_MatchLength == 3 && m_StringStart - m_MatchStart > 4096)
                    {
                        m_MatchLength = 2;
                    }
                }

                if (matchLength >= 3 && m_MatchLength <= matchLength)
                {
                    HuffmanTallyDist(m_StringStart - 1 - matchStart, matchLength);
                    matchLength -= 2;
                    do
                    {
                        m_StringStart++;
                        m_LookAhead--;
                        if (m_LookAhead >= 3)
                        {
                            InsertString();
                        }
                    }
                    while (--matchLength > 0);
                    m_StringStart++;
                    m_LookAhead--;
                    m_MatchPreviousAvailable = false;
                    m_MatchLength = 2;
                }
                else
                {
                    if (m_MatchPreviousAvailable)
                    {
                        HuffmanTallyLit(m_DataWindow[m_StringStart - 1] & 0xFF);
                    }

                    m_MatchPreviousAvailable = true;
                    m_StringStart++;
                    m_LookAhead--;
                }

                if (HuffmanIsFull)
                {
                    int num2 = m_StringStart - m_BlockStart;
                    if (m_MatchPreviousAvailable)
                    {
                        num2--;
                    }

                    bool flag = finish && m_LookAhead == 0 && !m_MatchPreviousAvailable;
                    HuffmanFlushBlock(m_DataWindow, m_BlockStart, num2, flag);
                    m_BlockStart += num2;
                    return !flag;
                }
            }

            return true;
        }

        //
        // Summary:
        //     CompressData drives actual compression of data
        private bool CompressData(bool finish)
        {
            bool flag;
            do
            {
                FillWindow();
                bool flush = finish && NeedsInput;
                flag = m_CompressionFunction switch
                {
                    0 => SaveStored(flush, finish),
                    1 => CompressFast(flush, finish),
                    2 => CompressSlow(flush, finish),
                    _ => throw new InvalidOperationException("unknown m_CompressionFunction"),
                };
            }
            while (PendingBufferIsFlushed && flag);
            return flag;
        }

        //
        // Summary:
        //     Reset internal state
        private void HuffmanReset()
        {
            m_iBufferPosition = 0;
            m_iExtraBits = 0;
            m_treeLiteral.Reset();
            m_treeDistances.Reset();
            m_treeCodeLengths.Reset();
        }

        //
        // Summary:
        //     Calculates length code from length.
        //
        // Parameters:
        //   len:
        //     Length.
        //
        // Returns:
        //     Length code.
        private int HuffmanLengthCode(int len)
        {
            if (len == 255)
            {
                return 285;
            }

            int num = 257;
            while (len >= 8)
            {
                num += 4;
                len >>= 1;
            }

            return num + len;
        }

        //
        // Summary:
        //     Calculates distance code from distance.
        //
        // Parameters:
        //   distance:
        //     Distance.
        //
        // Returns:
        //     Distance code.
        private int HuffmanDistanceCode(int distance)
        {
            int num = 0;
            while (distance >= 4)
            {
                num += 2;
                distance >>= 1;
            }

            return num + distance;
        }

        //
        // Summary:
        //     Write all trees to pending buffer
        private void HuffmanSendAllTrees(int blTreeCodes)
        {
            m_treeCodeLengths.BuildCodes();
            m_treeLiteral.BuildCodes();
            m_treeDistances.BuildCodes();
            PendingBufferWriteBits(m_treeLiteral.TreeLength - 257, 5);
            PendingBufferWriteBits(m_treeDistances.TreeLength - 1, 5);
            PendingBufferWriteBits(blTreeCodes - 4, 4);
            for (int i = 0; i < blTreeCodes; i++)
            {
                PendingBufferWriteBits(m_treeCodeLengths.CodeLengths[Utils.DEF_HUFFMAN_DYNTREE_CODELENGTHS_ORDER[i]], 3);
            }

            m_treeLiteral.WriteTree(m_treeCodeLengths);
            m_treeDistances.WriteTree(m_treeCodeLengths);
        }

        //
        // Summary:
        //     Compress current buffer writing data to pending buffer
        private void HuffmanCompressBlock()
        {
            for (int i = 0; i < m_iBufferPosition; i++)
            {
                int num = m_arrLiteralsBuffer[i] & 0xFF;
                int num2 = m_arrDistancesBuffer[i];
                if (num2-- != 0)
                {
                    int num3 = HuffmanLengthCode(num);
                    m_treeLiteral.WriteCodeToStream(num3);
                    int num4 = (num3 - 261) / 4;
                    if (num4 > 0 && num4 <= 5)
                    {
                        PendingBufferWriteBits(num & ((1 << num4) - 1), num4);
                    }

                    int num5 = HuffmanDistanceCode(num2);
                    m_treeDistances.WriteCodeToStream(num5);
                    num4 = num5 / 2 - 1;
                    if (num4 > 0)
                    {
                        PendingBufferWriteBits(num2 & ((1 << num4) - 1), num4);
                    }
                }
                else
                {
                    m_treeLiteral.WriteCodeToStream(num);
                }
            }

            m_treeLiteral.WriteCodeToStream(256);
        }

        //
        // Summary:
        //     Flush block to output with no compression
        //
        // Parameters:
        //   stored:
        //     Data to write
        //
        //   storedOffset:
        //     Index of first byte to write
        //
        //   storedLength:
        //     Count of bytes to write
        //
        //   lastBlock:
        //     True if this is the last block
        private void HuffmanFlushStoredBlock(byte[] stored, int storedOffset, int storedLength, bool lastBlock)
        {
            PendingBufferWriteBits(lastBlock ? 1 : 0, 3);
            PendingBufferAlignToByte();
            PendingBufferWriteShort(storedLength);
            PendingBufferWriteShort(~storedLength);
            PendingBufferWriteByteBlock(stored, storedOffset, storedLength);
            HuffmanReset();
        }

        //
        // Summary:
        //     Flush block to output with compression
        //
        // Parameters:
        //   stored:
        //     Data to flush
        //
        //   storedOffset:
        //     Index of first byte to flush
        //
        //   storedLength:
        //     Count of bytes to flush
        //
        //   lastBlock:
        //     True if this is the last block
        private void HuffmanFlushBlock(byte[] stored, int storedOffset, int storedLength, bool lastBlock)
        {
            m_treeLiteral.CodeFrequences[256]++;
            m_treeLiteral.BuildTree();
            m_treeDistances.BuildTree();
            m_treeLiteral.CalcBLFreq(m_treeCodeLengths);
            m_treeDistances.CalcBLFreq(m_treeCodeLengths);
            m_treeCodeLengths.BuildTree();
            int num = 4;
            for (int num2 = 18; num2 > num; num2--)
            {
                if (m_treeCodeLengths.CodeLengths[Utils.DEF_HUFFMAN_DYNTREE_CODELENGTHS_ORDER[num2]] > 0)
                {
                    num = num2 + 1;
                }
            }

            int num3 = 14 + num * 3 + m_treeCodeLengths.GetEncodedLength() + m_treeLiteral.GetEncodedLength() + m_treeDistances.GetEncodedLength() + m_iExtraBits;
            int num4 = m_iExtraBits;
            for (int i = 0; i < 286; i++)
            {
                num4 += m_treeLiteral.CodeFrequences[i] * m_arrLiteralLengths[i];
            }

            for (int j = 0; j < 30; j++)
            {
                num4 += m_treeDistances.CodeFrequences[j] * m_arrDistanceLengths[j];
            }

            if (num3 >= num4)
            {
                num3 = num4;
            }

            if (storedOffset >= 0 && storedLength + 4 < num3 >> 3)
            {
                HuffmanFlushStoredBlock(stored, storedOffset, storedLength, lastBlock);
            }
            else if (num3 == num4)
            {
                PendingBufferWriteBits(2 + (lastBlock ? 1 : 0), 3);
                m_treeLiteral.SetStaticCodes(m_arrLiteralCodes, m_arrLiteralLengths);
                m_treeDistances.SetStaticCodes(m_arrDistanceCodes, m_arrDistanceLengths);
                HuffmanCompressBlock();
                HuffmanReset();
            }
            else
            {
                PendingBufferWriteBits(4 + (lastBlock ? 1 : 0), 3);
                HuffmanSendAllTrees(num);
                HuffmanCompressBlock();
                HuffmanReset();
            }
        }

        //
        // Summary:
        //     Add literal to buffer.
        //
        // Parameters:
        //   literal:
        //
        // Returns:
        //     Value indicating internal buffer is full
        private bool HuffmanTallyLit(int literal)
        {
            m_arrDistancesBuffer[m_iBufferPosition] = 0;
            m_arrLiteralsBuffer[m_iBufferPosition++] = (byte)literal;
            m_treeLiteral.CodeFrequences[literal]++;
            return HuffmanIsFull;
        }

        //
        // Summary:
        //     Add distance code and length to literal and distance trees
        //
        // Parameters:
        //   dist:
        //     Distance code
        //
        //   len:
        //     Length
        //
        // Returns:
        //     Value indicating if internal buffer is full
        private bool HuffmanTallyDist(int dist, int len)
        {
            m_arrDistancesBuffer[m_iBufferPosition] = (short)dist;
            m_arrLiteralsBuffer[m_iBufferPosition++] = (byte)(len - 3);
            int num = HuffmanLengthCode(len - 3);
            m_treeLiteral.CodeFrequences[num]++;
            if (num >= 265 && num < 285)
            {
                m_iExtraBits += (num - 261) / 4;
            }

            int num2 = HuffmanDistanceCode(dist - 1);
            m_treeDistances.CodeFrequences[num2]++;
            if (num2 >= 4)
            {
                m_iExtraBits += num2 / 2 - 1;
            }

            return HuffmanIsFull;
        }

        //
        // Summary:
        //     write a byte to buffer
        //
        // Parameters:
        //   b:
        //     value to write
        internal void PendingBufferWriteByte(int b)
        {
            m_PendingBuffer[m_PendingBufferLength++] = (byte)b;
        }

        //
        // Summary:
        //     Write a short value to buffer LSB first
        //
        // Parameters:
        //   s:
        //     value to write
        internal void PendingBufferWriteShort(int s)
        {
            m_PendingBuffer[m_PendingBufferLength++] = (byte)s;
            m_PendingBuffer[m_PendingBufferLength++] = (byte)(s >> 8);
        }

        //
        // Summary:
        //     write an integer LSB first
        //
        // Parameters:
        //   s:
        //     value to write
        internal void PendingBufferWriteInt(int s)
        {
            m_PendingBuffer[m_PendingBufferLength++] = (byte)s;
            m_PendingBuffer[m_PendingBufferLength++] = (byte)(s >> 8);
            m_PendingBuffer[m_PendingBufferLength++] = (byte)(s >> 16);
            m_PendingBuffer[m_PendingBufferLength++] = (byte)(s >> 24);
        }

        //
        // Summary:
        //     Write a block of data to buffer
        //
        // Parameters:
        //   data:
        //     data to write
        //
        //   offset:
        //     offset of first byte to write
        //
        //   length:
        //     number of bytes to write
        internal void PendingBufferWriteByteBlock(byte[] data, int offset, int length)
        {
            Array.Copy(data, offset, m_PendingBuffer, m_PendingBufferLength, length);
            m_PendingBufferLength += length;
        }

        //
        // Summary:
        //     Align internal buffer on a byte boundary
        internal void PendingBufferAlignToByte()
        {
            if (m_PendingBufferBitsInCache > 0)
            {
                m_PendingBuffer[m_PendingBufferLength++] = (byte)m_PendingBufferBitsCache;
                if (m_PendingBufferBitsInCache > 8)
                {
                    m_PendingBuffer[m_PendingBufferLength++] = (byte)(m_PendingBufferBitsCache >> 8);
                }
            }

            m_PendingBufferBitsCache = 0u;
            m_PendingBufferBitsInCache = 0;
        }

        //
        // Summary:
        //     Write bits to internal buffer
        //
        // Parameters:
        //   b:
        //     source of bits
        //
        //   count:
        //     number of bits to write
        internal void PendingBufferWriteBits(int b, int count)
        {
            m_PendingBufferBitsCache |= (uint)(b << m_PendingBufferBitsInCache);
            m_PendingBufferBitsInCache += count;
            PendingBufferFlushBits();
        }

        //
        // Summary:
        //     Write a short value to internal buffer most significant byte first
        //
        // Parameters:
        //   s:
        //     value to write
        internal void PendingBufferWriteShortMSB(int s)
        {
            m_PendingBuffer[m_PendingBufferLength++] = (byte)(s >> 8);
            m_PendingBuffer[m_PendingBufferLength++] = (byte)s;
        }

        //
        // Summary:
        //     Flushes the pending buffer into the given output array. If the output array is
        //     to small, only a partial flush is done.
        internal void PendingBufferFlush()
        {
            PendingBufferFlushBits();
            m_stream.Write(m_PendingBuffer, 0, m_PendingBufferLength);
            m_PendingBufferLength = 0;
            m_stream.Flush();
        }

        //
        // Summary:
        //     Flushes fully recorded bytes to buffer array.
        //
        // Returns:
        //     Count of bytes, added to buffer.
        internal int PendingBufferFlushBits()
        {
            int num = 0;
            while (m_PendingBufferBitsInCache >= 8 && m_PendingBufferLength < 65536)
            {
                m_PendingBuffer[m_PendingBufferLength++] = (byte)m_PendingBufferBitsCache;
                m_PendingBufferBitsCache >>= 8;
                m_PendingBufferBitsInCache -= 8;
                num++;
            }

            return num;
        }

        //
        // Summary:
        //     Convert internal buffer to byte array. Buffer is empty on completion
        //
        // Returns:
        //     converted buffer contents contents
        internal byte[] PendingBufferToByteArray()
        {
            byte[] array = new byte[m_PendingBufferLength];
            Array.Copy(m_PendingBuffer, 0, array, 0, array.Length);
            m_PendingBufferLength = 0;
            return array;
        }
    }
}
