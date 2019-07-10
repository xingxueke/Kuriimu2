using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kompression.LempelZiv.Matcher;

namespace Kompression.LempelZiv
{
    public abstract class BaseLz : ICompression
    {
        protected abstract ILzDecoder Decoder { get; }
        protected abstract ILzMatcher Matcher { get; }
        protected abstract ILzEncoder Encoder { get; }

        public Stream Decompress(Stream input)
        {
            return Decoder.Decode(input);
        }

        public Stream Compress(Stream input)
        {
            var bkPos = input.Position;
            var matches = Matcher.FindMatches(input);
            input.Position = bkPos;
            return Encoder.Encode(input, matches);
        }
    }
}
