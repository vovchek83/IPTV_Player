using System;
using System.ComponentModel.Composition;
using System.IO;
using System.IO.Compression;
using IPTV_Player.Infrastructure.Services.Interfaces;

namespace IPTV.Infrastructure.Services
{
    [Export(typeof(IDecompressService))]
    public class DecompressService : IDecompressService
    {
        public void Decompress(FileInfo fileToDecompress)
        {
            using (FileStream originalFileStream = fileToDecompress.OpenRead())
            {
                string currentFileName = fileToDecompress.FullName;
                string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

                using (FileStream decompressedFileStream = File.Create(newFileName))
                {
                    using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(decompressedFileStream);
                        Console.WriteLine("Decompressed: {0}", fileToDecompress.Name);
                    }
                }
            }
        }
    }
}
