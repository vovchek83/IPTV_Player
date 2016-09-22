using System.IO;
using IPTV_Player.Infrastructure.Services.Interfaces;

namespace IPTV_Player.ViewModels
{
    public class ShellViewModel : Caliburn.Micro.PropertyChangedBase, IShell
    {
        private const string EPG_URL = "http://www.teleguide.info/download/new3/xmltv.xml.gz";

        public ShellViewModel(IDownloadService downloadService, IDecompressService decompressService)
        {
            string appFolder = downloadService.ApplicationDataFolder;

            downloadService.DaownloadFileAsync(EPG_URL, appFolder, "xmltv.xml.gz");

            //if (file != null)
            //{ 
            //    decompressService.Decompress(file);
            //}
        }
    }
}