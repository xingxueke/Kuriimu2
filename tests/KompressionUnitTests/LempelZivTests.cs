using System;
using System.IO;
using System.Linq;
using Kompression.LempelZiv;
using Kompression.LempelZiv.Matcher;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KompressionUnitTests
{
    [TestClass]
    public class LempelZivTests
    {
        private static readonly byte[] TestCorpus =
        {
            0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x70, 0x00, 0x00, 0x00,
            0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x04, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x01, 0x02, 0x02, 0x03, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x05, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x09, 0x00,
            0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x89, 0x00
        };

        private (byte[], byte[]) CompressDecompressInternal(Action<Stream, Stream> decompAction, Action<Stream, Stream> compAction)
        {
            var compStream = new MemoryStream();
            var decompStream = new MemoryStream();

            compAction(new MemoryStream(TestCorpus), compStream);
            compStream.Position = 0;
            decompAction(compStream, decompStream);
            decompStream.Position = compStream.Position = 0;

            return (decompStream.ToArray(), compStream.ToArray());
        }

        [TestMethod]
        public void LZ10_CompressDecompress()
        {
            var (decompressedData, compressedData) = CompressDecompressInternal(LZ10.Decompress, LZ10.Compress);

            Assert.AreEqual(0x10, compressedData[0]);
            Assert.IsTrue(TestCorpus.SequenceEqual(decompressedData));
        }

        [TestMethod]
        public void LZ11_CompressDecompress()
        {
            var (decompressedData, compressedData) = CompressDecompressInternal(LZ11.Decompress, LZ11.Compress);

            Assert.AreEqual(0x11, compressedData[0]);
            Assert.IsTrue(TestCorpus.SequenceEqual(decompressedData));
        }

        [TestMethod]
        public void LZ40_CompressDecompress()
        {
            var (decompressedData, compressedData) = CompressDecompressInternal(LZ40.Decompress, LZ40.Compress);

            Assert.AreEqual(0x40, compressedData[0]);
            Assert.IsTrue(TestCorpus.SequenceEqual(decompressedData));
        }

        [TestMethod]
        public void LZ60_CompressDecompress()
        {
            var (decompressedData, compressedData) = CompressDecompressInternal(LZ60.Decompress, LZ60.Compress);

            Assert.AreEqual(0x60, compressedData[0]);
            Assert.IsTrue(TestCorpus.SequenceEqual(decompressedData));
        }

        [TestMethod]
        public void LZ77_CompressDecompress()
        {
            var (decompressedData, compressedData) = CompressDecompressInternal(LZ77.Decompress, LZ77.Compress);

            Assert.IsTrue(TestCorpus.SequenceEqual(decompressedData));
        }

        [TestMethod]
        public void LZSS_CompressDecompress()
        {
            var (decompressedData, compressedData) = CompressDecompressInternal(LZSS.Decompress, LZSS.Compress);

            Assert.AreEqual(0x53, compressedData[0]);
            Assert.AreEqual(0x53, compressedData[1]);
            Assert.AreEqual(0x5A, compressedData[2]);
            Assert.AreEqual(0x4C, compressedData[3]);

            var decompressedSize = compressedData[0xC] | (compressedData[0xD] << 8) | (compressedData[0xE] << 16) | (compressedData[0xF] << 24);
            Assert.AreEqual(decompressedData.Length, decompressedSize);

            Assert.IsTrue(TestCorpus.SequenceEqual(decompressedData));
        }

        [TestMethod]
        public void LZSSVLC_CompressDecompress()
        {
            var (decompressedData, compressedData) = CompressDecompressInternal(LZSSVLC.Decompress, LZSSVLC.Compress);

            Assert.IsTrue(TestCorpus.SequenceEqual(decompressedData));
        }

        [TestMethod]
        public void Stub_LZSSVLC_SuffixTree_Compress()
        {
            var file = @"D:\Users\Kirito\Desktop\vt1.file1.bin";
            var str = File.OpenRead(file);
            var save = File.Create(file + ".new2");

            LZSSVLC.Compress(str, save);
        }

        [TestMethod]
        public void Stub_LZSSVLC_SuffixTree_Decompress()
        {
            var file = @"D:\Users\Kirito\Desktop\vt1.first_chunk.bin";
            var str = File.OpenRead(file);
            var save = File.Create(file + ".decomp");

            LZSSVLC.Decompress(str, save);
        }
    }
}
