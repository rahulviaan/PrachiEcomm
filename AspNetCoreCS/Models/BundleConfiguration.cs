using Ionic.Zip;
using ReadEdgeCore.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEdgeCore.Models
{
    public class BundleConfiguration : IBundleConfiguration
    {
        public async Task RecursiveDelete(DirectoryInfo baseDir)
        {
            if (!baseDir.Exists)
                return;

            foreach (var dir in baseDir.EnumerateDirectories())
            {

              await RecursiveDelete(dir);
            }
            baseDir.Delete(true);
        }

        public async Task<bool> UnzipBook(string zipSourcePath, string bookPath)
        {
            var status = false;
                DirectoryInfo dir = new DirectoryInfo(bookPath);
                if (dir.Exists)
                {
                  await  RecursiveDelete(dir);
                }

                dir.Create();
                var zipFile = ZipFile.Read(zipSourcePath);
                zipFile.ExtractAll(bookPath, ExtractExistingFileAction.OverwriteSilently);
                status = true;
            return status;
        }

        public async Task<bool> UploadBundles(string epubName, string bookPath, long bookId, string filename)
        {
            var result = false;

                var directoryname = filename.Split('.')[0];
                var epubDirectory = bookPath +"\\"+ directoryname + "\\";
                var isUnzipped =await UnzipBook(epubName, epubDirectory);

                if (isUnzipped)
                {
                    result = true;
                    var epubFullPath = string.Format("{0}{1}", epubDirectory, filename);
                    var zipFullPath = "";
                    var epubPath = string.Format("{0}{1}", epubDirectory, directoryname);

                    if (File.Exists(epubFullPath))
                    {
                        var ext = Path.GetExtension(epubFullPath);
                        zipFullPath = epubFullPath.Replace(ext, ".zip");
                        File.Copy(epubFullPath, zipFullPath, true);
                        File.Delete(epubFullPath);
                    }

                    if (File.Exists(zipFullPath))
                    {
                        result =await UnzipBook(zipFullPath, epubPath);

                        if (result)
                        {
                            GC.Collect();
                            GC.WaitForPendingFinalizers();

                            File.Delete(zipFullPath);
                        }
                    }
     
            }
            return result;
        }
    }
}
