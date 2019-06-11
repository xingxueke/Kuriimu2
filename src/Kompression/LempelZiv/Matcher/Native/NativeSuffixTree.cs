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
        private const string _dllPath = @"Libraries\ukkonen.dll";
        private const string _dllPathUnix = @"Libraries\ukkonen.so";

        [DllImport(_dllPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "CreateSuffixTree")]
        public static extern IntPtr CreateSuffixTree();

        [DllImport(_dllPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "DestroySuffixTree")]
        public static extern void DestroySuffixTree(IntPtr tree);

        [DllImport(_dllPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "Build")]
        public static extern IntPtr Build(IntPtr tree, IntPtr input, int position, int size);

        [DllImport(_dllPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "FindLongestMatch")]
        public static extern void FindLongestMatch(IntPtr tree, IntPtr input, int position, int size, IntPtr displacement, IntPtr length);

        [DllImport(_dllPathUnix, CallingConvention = CallingConvention.Cdecl, EntryPoint = "CreateSuffixTree")]
        public static extern IntPtr CreateSuffixTreeUnix();

        [DllImport(_dllPathUnix, CallingConvention = CallingConvention.Cdecl, EntryPoint = "DestroySuffixTree")]
        public static extern void DestroySuffixTreeUnix(IntPtr tree);

        [DllImport(_dllPathUnix, CallingConvention = CallingConvention.Cdecl, EntryPoint = "Build")]
        public static extern IntPtr BuildUnix(IntPtr tree, IntPtr input, int position, int size);

        [DllImport(_dllPathUnix, CallingConvention = CallingConvention.Cdecl, EntryPoint = "FindLongestMatch")]
        public static extern void FindLongestMatchUnix(IntPtr tree, IntPtr input, int position, int size, IntPtr displacement, IntPtr length);
    }
}
