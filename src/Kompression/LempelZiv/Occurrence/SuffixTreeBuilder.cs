using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml;
using Kompression.LempelZiv.Occurrence.Models;

/* A suffix tree parses a file into patterns which are then directly accessible through a root node.
   The time used for finding a pattern is then O(n).
   See this article series for explanation and implementation of Ukkonen's suffix tree creation:
   https://www.geeksforgeeks.org/ukkonens-suffix-tree-construction-part-6/ */

namespace Kompression.LempelZiv.Occurrence
{
    unsafe class SuffixTreeBuilder
    {
        private byte[] _inputArray;

        SuffixTreeNode _root;
        private SuffixTreeNode _lastNewNode;
        SuffixTreeNode _activeNode;

        // Position in input
        int _activeEdge = -1;
        int _activeLength;

        int _remainingSuffixCount;
        int* _leafEnd;
        int _size = -1;

        private int* _rootEnd = null;
        private int* _splitEnd = null;

        private SuffixTreeNode CreateNewNode(int start, int* end)
        {
            var node = new SuffixTreeNode();

            node.Children = new IntPtr[256];
            node.Start = start;
            node.End = end;
            node.SuffixIndex = -1;
            if (_root != null)
            {
                node.SuffixLink = Marshal.AllocHGlobal(0x410);
                Marshal.StructureToPtr(_root, node.SuffixLink, false);
            }
            else
                node.SuffixLink = IntPtr.Zero;

            return node;

            /* Node* node = (Node*)malloc(sizeof(Node));
	*memorySize += sizeof(*node);
	int i;
	for (i = 0; i < MAX_CHAR; i++)
		node->children[i] = nullptr;

	/*For root node, suffixLink will be set to nullptr
	For internal nodes, suffixLink will be set to root
	by default in  current extension and may change in
	next extension
            node->suffixLink = root;
            node->start = start;
            node->end = end;

            /*suffixIndex will be set to -1 by default and
              actual suffix index will be set later for leaves
              at the end of all phases
            node->suffixIndex = -1;
            return node; */
        }

        public SuffixTreeNode Build(Stream input)
        {
            var bkPos = input.Position;
            _inputArray = new byte[input.Length];
            input.Read(_inputArray, 0, _inputArray.Length);
            input.Position = bkPos;

            _rootEnd = (int*)Marshal.AllocHGlobal(4);

            _leafEnd = (int*)Marshal.AllocHGlobal(4);
            _size = (int)input.Length;
            _root = CreateNewNode(-1, _rootEnd);

            _activeNode = _root;
            for (int i = 0; i < _size; i++)
                ExtendTree(i);

            //var labelHeight = 0;
            //SetSuffixIndexByDFS(_root, labelHeight);

            return _root;
        }

        private void ExtendTree(int pos)
        {
            *_leafEnd = pos;
            _remainingSuffixCount++;
            _lastNewNode = null;

            while (_remainingSuffixCount > 0)
            {
                if (_activeLength == 0)
                    _activeEdge = pos;

                if (_activeNode.Children[_inputArray[_activeEdge]] == IntPtr.Zero)
                {
                    _activeNode.Children[_inputArray[_activeEdge]] = Marshal.AllocHGlobal(0x410);
                    Marshal.StructureToPtr(CreateNewNode(pos, _leafEnd), _activeNode.Children[_inputArray[_activeEdge]], true);

                    if (_lastNewNode != null)
                    {
                        Marshal.StructureToPtr(_activeNode, _lastNewNode.SuffixLink, false);
                        _lastNewNode = null;
                    }
                }
                else
                {
                    SuffixTreeNode next = new SuffixTreeNode();
                    Marshal.PtrToStructure(_activeNode.Children[_inputArray[_activeEdge]], next);
                    if (TryWalkDown(next))
                        continue;

                    if (_inputArray[next.Start + _activeLength] == _inputArray[pos])
                    {
                        if (_lastNewNode != null && _activeNode != _root)
                        {
                            Marshal.StructureToPtr(_activeNode, _lastNewNode.SuffixLink, false);
                            _lastNewNode = null;
                        }

                        _activeLength++;
                        break;
                    }

                    _splitEnd = (int*)Marshal.AllocHGlobal(4);
                    *_splitEnd = next.Start + _activeLength - 1;

                    var split = CreateNewNode(next.Start, _splitEnd);
                    Marshal.StructureToPtr(split, _activeNode.Children[_inputArray[_activeEdge]], false);

                    Marshal.StructureToPtr(CreateNewNode(pos, _leafEnd), split.Children[_inputArray[pos]], false);

                    next.Start += _activeLength;
                    Marshal.StructureToPtr(next, split.Children[_inputArray[next.Start]], false);

                    if (_lastNewNode != null)
                        Marshal.StructureToPtr(split, _lastNewNode.SuffixLink, false);

                    _lastNewNode = split;
                }

                _remainingSuffixCount--;
                if (_activeNode == _root && _activeLength > 0)
                {
                    _activeLength--;
                    _activeEdge = pos - _remainingSuffixCount + 1;
                }
                else if (_activeNode != _root)
                    Marshal.PtrToStructure(_activeNode.SuffixLink, _activeNode);
            }
        }

        private int GetLength(SuffixTreeNode node)
        {
            return *node.End - node.Start + 1;
        }

        private bool TryWalkDown(SuffixTreeNode node)
        {
            if (_activeLength < GetLength(node))
                return false;

            _activeEdge += GetLength(node);
            _activeLength -= GetLength(node);
            _activeNode = node;
            return true;
        }

        //private void SetSuffixIndexByDFS(SuffixTreeNode node, int labelHeight)
        //{
        //    if (node == null) return;

        //    var leaf = true;
        //    for (int i = 0; i < 256; i++)
        //    {
        //        if (node.Children.Any(x => x.Index == i))
        //        {
        //            leaf = false;
        //            var first = node.Children.First(x => x.Index == i);
        //            SetSuffixIndexByDFS(first.Node, labelHeight + first.Node.Length);
        //        }
        //    }

        //    if (leaf)
        //        node.SuffixIndex = _size - labelHeight;
        //}
    }
}
