using System;
using System.Collections.Generic;
using System.IO;
using Kompression.LempelZiv.Matcher.Models;

namespace Kompression.LempelZiv.Matcher
{
    public class SuffixTreeMatcher : ILzMatcher
    {
        private readonly SuffixTree _tree;

        public SuffixTreeMatcher()
        {
            _tree = new SuffixTree();
        }

        public LzResult[] FindGreedyMatches(Stream input)
        {
            var results = new List<LzResult>();

            var inputArray = ToArray(input);

            _tree.Build(inputArray, (int)input.Position);

            for (var i = Math.Max((int)input.Position, 1); i < input.Length; i++)
            {
                var displacement = 0;
                var length = 0;
                _tree.FindLongestMatch(inputArray, i, ref displacement, ref length);
                if (displacement > 0 && length > 0)
                {
                    results.Add(new LzResult(i, displacement, length, null));
                    i += length - 1;
                }
            }

            return results.ToArray();
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
            if (disposing)
                _tree.Dispose();
        }
    }
}
