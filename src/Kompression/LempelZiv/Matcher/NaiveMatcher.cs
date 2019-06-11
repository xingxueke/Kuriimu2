using System;
using System.Collections.Generic;
using System.IO;
using Kompression.LempelZiv.Matcher.Models;

namespace Kompression.LempelZiv.Matcher
{
    unsafe class NaiveMatcher : ILzMatcher
    {
        public int MinOccurrenceSize { get; }

        public int MaxOccurrenceSize { get; }

        public int WindowSize { get; }

        public int MinDiscrepancySize { get; }

        public NaiveMatcher(int minOccurrence, int maxOccurrence, int windowSize, int minDiscrepancy)
        {
            MinOccurrenceSize = minOccurrence;
            MaxOccurrenceSize = maxOccurrence;
            WindowSize = windowSize;
            MinDiscrepancySize = minDiscrepancy;
        }

        public IList<LzResult> FindMatches(Stream input)
        {
            var result = new List<LzResult>();

            fixed (byte* ptr = ToArray(input))
            {
                var position = ptr;
                position += MinOccurrenceSize;

                while (position - ptr < input.Length)
                {
                    var displacementPtr = position - Math.Min(position - ptr, WindowSize);

                    var displacement = -1L;
                    var length = -1;
                    byte[] discrepancy = null;
                    while (displacementPtr < position)
                    {
                        if (length >= MaxOccurrenceSize)
                            break;

                        #region Find max occurence from displacementPtr onwards

                        var walk = 0;
                        while (*(displacementPtr + walk) == *(position + walk))
                        {
                            walk++;
                            if (walk >= MaxOccurrenceSize || position - ptr + walk >= input.Length)
                                break;
                        }

                        if (walk >= MinOccurrenceSize && walk > length)
                        {
                            length = walk;
                            displacement = position - displacementPtr;
                            discrepancy = new byte[MinDiscrepancySize];
                            for (int i = 0; i < discrepancy.Length; i++)
                                discrepancy[i] = *(position + walk + i);
                        }

                        #endregion

                        displacementPtr++;
                    }

                    if (length >= MinOccurrenceSize)
                    {
                        result.Add(new LzResult(position - ptr, displacement, length, discrepancy));
                        position += length + MinDiscrepancySize;
                    }
                    else
                    {
                        position++;
                    }
                }
            }

            return result;
        }

        private byte[] ToArray(Stream input)
        {
            var bkPos = input.Position;
            var inputArray = new byte[input.Length];
            input.Read(inputArray, 0, inputArray.Length);
            input.Position = bkPos;

            return inputArray;
        }
    }
}
