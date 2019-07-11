using System.IO;

namespace Kompression.LempelZiv.Decoders
{
    public interface ILzDecoder
    {
        Stream Decode(Stream input);
    }
}
