using System.IO;

namespace Kompression.LempelZiv.Encoders
{
    public interface ILzEncoder
    {
        Stream Encode(Stream input);
    }
}
