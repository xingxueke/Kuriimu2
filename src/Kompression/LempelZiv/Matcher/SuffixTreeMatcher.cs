using System;
using System.Collections.Generic;
using System.IO;
using Kompression.LempelZiv.Matcher.Native;
using Kompression.LempelZiv.Occurrence.Models;

namespace Kompression.LempelZiv.Matcher
{
    class SuffixTreeMatcher : ILzMatcher
    {
        private IntPtr _tree;

        public IList<LzResult> FindMatches(Stream input)
        {
            var results = new List<LzResult>();

            var inputArray = ToArray(input);

            CreateTree();
            BuildTree(inputArray, (int)input.Position);

            for (var i = (int)input.Position; i < input.Length; i++)
            {
                var match = FindMatch(inputArray, i);
                if (match.displacement > 0 && match.length > 0)
                {
                    results.Add(new LzResult(i, match.displacement, match.length, null));
                    i += match.length;
                }
            }

            FreeTree();
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

        private void CreateTree()
        {
            _tree = IsLinux() ? NativeSuffixTree.CreateSuffixTreeUnix() : NativeSuffixTree.CreateSuffixTree();
        }

        private void FreeTree()
        {
            if (IsLinux())
                NativeSuffixTree.DestroySuffixTreeUnix(_tree);
            else
                NativeSuffixTree.DestroySuffixTree(_tree);
        }

        private unsafe void BuildTree(byte[] input, int position)
        {
            fixed (byte* ptr = input)
            {
                if (IsLinux())
                    NativeSuffixTree.BuildUnix(_tree, (IntPtr)ptr, position, input.Length - position);
                else
                    NativeSuffixTree.Build(_tree, (IntPtr)ptr, position, input.Length - position);
            }
        }

        private unsafe (int displacement, int length) FindMatch(byte[] input, int position)
        {
            var displacement = new IntPtr(0);
            var length = new IntPtr(0);

            fixed (byte* ptr = input)
            {
                if (IsLinux())
                    NativeSuffixTree.FindLongestMatchUnix(_tree, (IntPtr)ptr, position, input.Length - position, displacement, length);
                else
                    NativeSuffixTree.FindLongestMatch(_tree, (IntPtr)ptr, position, input.Length - position, displacement, length);
            }

            return (displacement.ToInt32(), length.ToInt32());
        }

        private static bool IsLinux()
        {
            var p = (int)Environment.OSVersion.Platform;
            return p == 4 || p == 6 || p == 128;
        }
    }
}
