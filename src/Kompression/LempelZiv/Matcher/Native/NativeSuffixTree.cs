using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Kompression.LempelZiv.Matcher.Native
{
    internal class NativeSuffixTree
    {
        private const string DllPath = @"Libraries\ukkonen";

        [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "CreateSuffixTree")]
        public static extern IntPtr CreateSuffixTree();

        [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "DestroySuffixTree")]
        public static extern void DestroySuffixTree(IntPtr tree);

        [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "Build")]
        private static extern void Build(IntPtr tree, IntPtr input, int position, int size);

        [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "FindLongestMatch")]
        private static extern void FindLongestMatch(IntPtr tree, IntPtr input, int position, int size, IntPtr displacement, IntPtr length);

        public static unsafe void BuildSuffixTree(IntPtr tree, byte[] input, int position)
        {
            fixed (byte* ptr = input)
                Build(tree, (IntPtr)ptr, position, input.Length);
        }

        public static unsafe (int displacement, int length) FindLongestMatch(IntPtr tree, byte[] input, int position)
        {
            var displacement = 0;
            var length = 0;

            fixed (byte* ptr = input)
                FindLongestMatch(tree, (IntPtr)ptr, position, input.Length, new IntPtr(&displacement), new IntPtr(&length));

            return (displacement, length);
        }
    }
}
