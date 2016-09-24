using System.IO;
using System.Threading.Tasks;

namespace IPTV_Player.Infrastructure.Services.Interfaces
{
    public interface IDownloadService
    {
        string ApplicationDataFolder { get; }

        string DownloadFile(string url,string fileName);

        void DaownloadFileAsync(string url,string fileName);
    }
}