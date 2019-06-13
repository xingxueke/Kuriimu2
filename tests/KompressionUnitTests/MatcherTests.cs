using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kompression.LempelZiv.Matcher;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KompressionUnitTests
{
    [TestClass]
    public class MatcherTests
    {
        [TestMethod]
        public void FindMatches_Naive_IsCorrect()
        {
            var input = new byte[]
                {0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00};

            var lzFinder = new NaiveMatcher(4, 16, 8, 0);
            var results = lzFinder.FindMatches(new MemoryStream(input));

            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(7, results[0].Position);
            Assert.AreEqual(8, results[0].Length);
            Assert.AreEqual(7, results[0].Displacement);
        }

        [TestMethod]
        public void FindMatches_SuffixTree_IsCorrect()
        {
            var input = new byte[]
                {0x00, 0x01, 0x02, 0x02, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x02, 0x03, 0x00, 0x00, 0x00};

            var lzFinder = new SuffixTreeMatcher();
            var results = lzFinder.FindMatches(new MemoryStream(input));

            Assert.AreEqual(5, results.Count);
            Assert.AreEqual(12, (int)results[4].Position);
            Assert.AreEqual(8, (int)results[4].Displacement);
            Assert.AreEqual(3, results[4].Length);
        }

        [TestMethod]
        public void FindMatches_SuffixArray_IsCorrect()
        {
            // TODO
        }
    }
}
