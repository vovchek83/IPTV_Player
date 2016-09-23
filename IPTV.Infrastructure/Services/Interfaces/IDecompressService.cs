using System.IO;

namespace IPTV_Player.Infrastructure.Services.Interfaces
{
    public interface IDecompressService
    {
        void Decompress(FileInfo fileToDecompress);
    }
}