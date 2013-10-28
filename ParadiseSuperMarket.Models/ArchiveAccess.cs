using System;
using System.IO.Compression;

namespace ParadiseSuperMarket.Models
{
    public class ArchiveAccess
    {
        public string ZipPath { get; set; }
        public string ExtractPath { get; set; }

        public ArchiveAccess(string zipPath, string extractPath = "/temp")
        {
            this.ZipPath = zipPath;
            this.ExtractPath = extractPath;
        }

        public void Extract()
        {
            ZipFile.ExtractToDirectory(this.ZipPath, this.ExtractPath);
        }
    }
}
