using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEdgeCore.Models.Interfaces
{
    public interface IBundleConfiguration
    {
        Task<bool> UploadBundles(string epubName, string bookPath, long bookId, string filename);
        Task<bool> UnzipBook(string zipSourcePath, string bookPath);
        Task RecursiveDelete(DirectoryInfo baseDir);
    }
}
