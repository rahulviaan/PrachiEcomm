using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace ReadEdgeCore.Models.Interfaces
{
    public interface IReaderBookService
    {
        void AddNote(objNote objnote);
        void BookStaticCreate(string BookStaticsPath, int firstIdex, int FontSize, string BgColor, string FontColor, double BrightNess, string zoomin);
        void BookStaticRead(string BookStaticsPath, out int currentindex, out int FontSize, out string BgColor, out string FontColor, out double BrightNess, out string zoomin);
        void BookStaticUpdate(string BookStaticsPath, int firstIdex, int FontSize, string BgColor, string FontColor, double BrightNess, string zoomin);
        void CreateBookMark(string BookStaticsPath, int currentindex, string Title);
        void DeleteNote(objNote objnote);
        IList<ReaderBookPage> Pages(XmlDocument xmldoc, string rootpath);
        IList<objBookMark> ReadBookMark(string BookStaticsPath);
        IList<objNote> ReadBookNotes(string BookStaticsPath);
        IList<ReaderBookPage> ReadFromXML(ReaderBooks book, string xmlFile, string rootpath);
        IList<ReaderBookIndex> ReadIndexFromXML(out string IndexTitle, string rootpath);
        objNote ReadNote(string BookStaticsPath, string key);
        IList<ReaderBookIndex> ReadRecuursive(string rootpath, XmlNode xn);
        IList<ReaderBookPage> ReadXMLFrom(UserReaderBookItem book, string xmlFile, string rootpath);
        void RemoveBookMark(string BookStaticsPath, int currentindex, string Title);
        string SetCoverPage(string rootpath);
        string SetLastPage(string rootpath);
        void UpdateNote(objNote objnote);
        bool ValidateBook(string bookpath);
    }
}
