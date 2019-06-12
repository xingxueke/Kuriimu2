using System;
using System.Collections.Generic;
using System.IO;
using Kompression.LempelZiv.Matcher.Models;
using Kompression.LempelZiv.Matcher.Native;

namespace Kompression.LempelZiv.Matcher
{
    public class SuffixTreeMatcher : ILzMatcher
    {
        private IntPtr _tree;

        public IList<LzResult> FindMatches(Stream input)
        {
            var results = new List<LzResult>();

            var inputArray = ToArray(input);

            _tree = NativeSuffixTree.CreateSuffixTree();
            NativeSuffixTree.BuildSuffixTree(_tree, inputArray, (int)input.Position);

            for (var i = Math.Max((int)input.Position, 1); i < input.Length; i++)
            {
                var match = NativeSuffixTree.FindLongestMatch(_tree, inputArray, i);
                if (match.displacement > 0 && match.length > 0)
                {
                    results.Add(new LzResult(i, match.displacement, match.length, null));
                    i += match.length - 1;
                }
            }

            return results;
        }

        private byte[] ToArray(Stream input)
        {
            var bkPos = input.Position;
            var inputArray = new byte[input.Length];
            input.Read(inputArray, 0, inputArray.Length);
            input.Position = bkPos;

            return inputArray;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected void Dispose(bool disposing)
        {
            if (!disposing)
            {
                if (_tree != IntPtr.Zero)
                    NativeSuffixTree.DestroySuffixTree(_tree);
                _tree = IntPtr.Zero;
            }
        }

        ~SuffixTreeMatcher()
        {
            Dispose(false);
        }
    }
}
