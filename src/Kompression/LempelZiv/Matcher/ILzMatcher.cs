using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kompression.LempelZiv.Matcher.Models;

namespace Kompression.LempelZiv.Matcher
{
    public interface ILzMatcher : IDisposable
    {
        IList<LzResult> FindMatches(Stream input);
    }
}
