using System;
using System.IO;
using System.Threading.Tasks;

namespace IdentifyFaces.Interface
{
    public interface IPicturePicker
    {
        Task<Stream> GetImageStreamAsync();
    }
}
