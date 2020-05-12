using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadEdgeCore.Models.Interfaces
{
    public interface IEbookReader
    {
        ReaderBooks GetReaderBooks(string fileName, string EncKey);
        string LoadPage(int pageID);
        string OpenEbook();
        string OpenPage(string pageReference);
    }
}
