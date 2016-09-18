using System.IO;
using System.Threading.Tasks;

namespace IPTV_Player.Infrastructure.Services.Interfaces
{
    public interface IDownloadService
    {
        FileInfo DownloadFile(string url, string directory, string fileName);

        void DaownloadFileAsync(string url, string directory, string fileName);
    }
}