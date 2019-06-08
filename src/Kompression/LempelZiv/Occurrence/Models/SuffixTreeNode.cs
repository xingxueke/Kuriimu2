using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Kompression.LempelZiv.Occurrence.Models
{
    [DebuggerDisplay("Length: {End.Value - Start + 1}")]
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    unsafe class SuffixTreeNode
    {
        [FieldOffset(0)]
        public IntPtr[] Children;

        [FieldOffset(256 * 4)]
        public IntPtr SuffixLink;

        [FieldOffset(256 * 4 + 4)]
        public int Start;

        [FieldOffset(256 * 4 + 8)]
        public int* End;

        [FieldOffset(256 * 4 + 12)]
        public int SuffixIndex;

        //public bool IsRoot => Start == -1 && End.Value == -1;

        //public bool IsLeaf => SuffixIndex >= 0;

        //public int Length => End.Value - Start + 1;

        // Path label is the combination of values from start to end inclusive of this node
    }
}
