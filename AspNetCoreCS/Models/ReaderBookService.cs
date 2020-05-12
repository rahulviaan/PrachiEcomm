using Microsoft.AspNetCore.Hosting;
using ReadEdgeCore.Models.Interfaces;
using ReadEdgeCore.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace ReadEdgeCore.Models
{
    public class ReaderBookService : IReaderBookService
    {
        private static  IHostingEnvironment _hostingEnvironment;

        public ReaderBookService(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public bool ValidateBook(string bookpath)
        {
            var IsValid = false;
            try
            {
                foreach (string fl in Directory.GetFiles(bookpath, "*.opf", SearchOption.AllDirectories))
                {
                    StreamReader reader = new StreamReader(fl);
                    string stEn = reader.ReadToEnd();
                    reader.Close();

                    var metadata = stEn.Substring(stEn.IndexOf("<metadata"), stEn.IndexOf("</metadata>") - stEn.IndexOf("<metadata")) + "<dtmcreate>" + DateTime.Now.ToString() + "</dtmcreate></metadata>";
                    metadata = metadata.Replace("<dc:", "<");
                    metadata = metadata.Replace("</dc:", "</");
                    var pagename = stEn.Substring(stEn.IndexOf("<manifest>"), stEn.IndexOf("</manifest>") - stEn.IndexOf("<manifest>")) + "</manifest>";
                    var navigation = stEn.Substring(stEn.IndexOf("<spine toc=\"ncx\">"), stEn.IndexOf("</spine>") - stEn.IndexOf("<spine toc=\"ncx\">")) + "</spine>";
                    StringBuilder sb = new StringBuilder(500);
                    sb.Append("<?xml version='1.0' encoding='UTF-8' ?>" + Environment.NewLine);
                    sb.Append("<root>" + Environment.NewLine);
                    sb.Append(metadata + Environment.NewLine + pagename + Environment.NewLine + navigation + Environment.NewLine);
                    sb.Append("</root>" + Environment.NewLine);
                    string stxml = sb.ToString();
                    StreamWriter writere = new StreamWriter(fl.Replace(".opf", ".xml"));
                    writere.Write(stxml);
                    writere.Close();
                    System.IO.File.Delete(fl);
                    foreach (string indexfile in Directory.GetFiles(bookpath, "*.ncx", SearchOption.AllDirectories))
                    {

                        StreamReader indexreader = new StreamReader(indexfile);
                        string indexstEn = indexreader.ReadToEnd();
                        indexreader.Close();
                        var navigationindex = indexstEn.Substring(indexstEn.IndexOf("<docTitle>"), indexstEn.IndexOf("</ncx>") - indexstEn.IndexOf("<docTitle>")).Replace("</ncx>", "");

                        StringBuilder sbindex = new StringBuilder(500);
                        sbindex.Append("<?xml version='1.0' encoding='UTF-8' ?>" + Environment.NewLine);
                        sbindex.Append("<root>" + Environment.NewLine);
                        sbindex.Append(navigationindex + Environment.NewLine);
                        sbindex.Append("</root>" + Environment.NewLine);
                        string stxmlindex = sbindex.ToString();
                        StreamWriter writereindex = new StreamWriter(indexfile.Replace(".ncx", ".xml"));
                        writereindex.Write(stxmlindex);
                        writereindex.Close();
                        System.IO.File.Delete(indexfile);

                    }


                    var preBook = bookpath.Replace("temp_", "");
                    DirectoryInfo dir = new DirectoryInfo(preBook);
                    if (dir.Exists)
                    {
                        RecursiveDelete(dir);
                    }
                    System.IO.Directory.Move(bookpath, preBook);

                }
            }
            catch (Exception ex)
            {


            }
            return IsValid;
        }
        public void AddNote(objNote objnote)
        {

            if (File.Exists(objnote.BookStaticsPath))
            {
                StringBuilder sb = new StringBuilder(500);
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(objnote.BookStaticsPath);
                var note = xmldoc.SelectSingleNode("/root/notes/note[@key='" + objnote.key + "']");
                XmlNode NodeItem = xmldoc.SelectSingleNode("/root/notes");
                if (note != null)
                {
                    if (string.IsNullOrWhiteSpace(objnote.notetext))
                    {
                        NodeItem.RemoveChild(note);
                        xmldoc.Save(objnote.BookStaticsPath);
                    }
                    else
                        UpdateNote(objnote);

                }
                else if (!string.IsNullOrWhiteSpace(objnote.notetext))
                {
                    XmlElement notenode = xmldoc.CreateElement("note");
                    notenode.SetAttribute("key", objnote.key.ToString());
                    notenode.SetAttribute("currentpage", objnote.currentindex.ToString());
                    notenode.SetAttribute("title", objnote.Title);
                    notenode.SetAttribute("selectedtext", objnote.selectedtext);
                    notenode.SetAttribute("notetext", objnote.notetext);
                    notenode.SetAttribute("date", DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString() + " : " + DateTime.Now.Hour.ToString() + " : " + DateTime.Now.Minute.ToString() + " : " + DateTime.Now.Second.ToString());
                    NodeItem.AppendChild(notenode);
                    xmldoc.Save(objnote.BookStaticsPath);
                }
            }
        }
        public void UpdateNote(objNote objnote)
        {
            if (File.Exists(objnote.BookStaticsPath))
            {
                StringBuilder sb = new StringBuilder(500);
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(objnote.BookStaticsPath);
                XmlNode note = xmldoc.SelectSingleNode("/root/notes/note[@key='" + objnote.key + "']");
                if (note != null)
                {
                    note.Attributes["key"].InnerText = objnote.key.ToString();
                    note.Attributes["currentpage"].InnerText = objnote.currentindex.ToString();
                    note.Attributes["title"].InnerText = objnote.Title.ToString();
                    note.Attributes["selectedtext"].InnerText = objnote.selectedtext.ToString();
                    note.Attributes["notetext"].InnerText = objnote.notetext.ToString();
                    note.Attributes["date"].InnerText = DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString() + " : " + DateTime.Now.Hour.ToString() + " : " + DateTime.Now.Minute.ToString() + " : " + DateTime.Now.Second.ToString();
                    xmldoc.Save(objnote.BookStaticsPath);
                }
            }
        }
        public void DeleteNote(objNote objnote)
        {
            try
            {
                if (File.Exists(objnote.BookStaticsPath))
                {
                    StringBuilder sb = new StringBuilder(500);
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.Load(objnote.BookStaticsPath);
                    var note = xmldoc.SelectNodes("/root/notes/note[@key='" + objnote.key + "']");
                    if (note != null)
                    {
                        XmlNode NodeItem = xmldoc.SelectSingleNode("/root/notes");
                        NodeItem.RemoveChild(note[0]);
                        xmldoc.Save(objnote.BookStaticsPath);
                    }
                }
            }
            catch { }
        }
        public objNote ReadNote(string BookStaticsPath, string key)
        {
            objNote objnote = new objNote();
            if (File.Exists(BookStaticsPath))
            {
                StringBuilder sb = new StringBuilder(500);
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(BookStaticsPath);

                XmlNode note = xmldoc.SelectSingleNode("/root/notes/note[@key='" + key + "']");
                if (note != null)
                {
                    objnote.key = note.Attributes["key"].InnerText;
                    objnote.currentindex = Common.ToSafeInt(note.Attributes["currentpage"].InnerText);
                    objnote.Title = note.Attributes["title"].InnerText;
                    objnote.selectedtext = note.Attributes["selectedtext"].InnerText;
                    objnote.notetext = note.Attributes["notetext"].InnerText;
                    objnote.dtmUpdate = note.Attributes["date"].InnerText;
                }
                else
                {
                    objnote = null;
                }
            }
            else { objnote = null; }
            return objnote;
        }
        public void RemoveBookMark(string BookStaticsPath, int currentindex, string Title)
        {
            if (File.Exists(BookStaticsPath))
            {
                StringBuilder sb = new StringBuilder(500);
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(BookStaticsPath);
                var bookmarknode = xmldoc.SelectNodes("/root/bookmarks/bookmark[@currentpage='" + currentindex + "']");
                if (bookmarknode != null && bookmarknode.Count > 0)
                {
                    XmlNode NodeItem = xmldoc.SelectSingleNode("/root/bookmarks");
                    NodeItem.RemoveChild(bookmarknode[0]);
                    xmldoc.Save(BookStaticsPath);
                }
            }
        }
        public static void RecursiveDelete(DirectoryInfo baseDir)
        {
            if (!baseDir.Exists)
                return;

            foreach (var dir in baseDir.EnumerateDirectories())
            {

                RecursiveDelete(dir);
            }
            baseDir.Delete(true);
        }
        public static void DeleteAllFiles(DirectoryInfo baseDir)
        {
            try
            {
                if (!baseDir.Exists)
                    return;
                foreach (var file in baseDir.EnumerateFiles())
                {

                    file.Delete();
                }
            }
            catch { }
        }

        public static void DeleteTempFiles(DirectoryInfo baseDir)
        {
            try
            {
                var files = baseDir.GetFiles("*.htm");
                foreach (var file in files)
                {
                    file.Delete();

                }
            }
            catch { }



        }

        public IList<ReaderBookPage> ReadFromXML(ReaderBooks book, string xmlFile, string rootpath)
        {
            List<ReaderBookPage> pages = new List<ReaderBookPage>();
            try
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(xmlFile);
                XmlNodeList xnList = xmldoc.SelectNodes("/root/spine/itemref");
                var dctitle = xmldoc.SelectSingleNode("/root/metadata/title").InnerXml;
                var dccreator = xmldoc.SelectSingleNode("/root/metadata/creator").InnerXml;
                var dcpublisher = xmldoc.SelectSingleNode("/root/metadata/publisher").InnerXml;
                var dcidentifier = xmldoc.SelectSingleNode("/root/metadata/identifier").InnerXml;
                var dclanguage = xmldoc.SelectSingleNode("/root/metadata/language").InnerXml;
                var dcdate = xmldoc.SelectSingleNode("/root/metadata/date").InnerXml;
                var dtmcreate = xmldoc.SelectSingleNode("/root/metadata/dtmcreate").InnerXml;

                book.dccreator = dccreator;
                book.dcpublisher = dcpublisher;
                book.dcidentifier = dcidentifier;
                book.dclanguage = dclanguage;
                book.dcdate = dcdate;
                book.dtmCreate = Common.ToSafeDate(dtmcreate);

                int i = 0;
                foreach (XmlNode x in xnList)
                {
                    var Title = Common.ToSafeString(x.Attributes["idref"].InnerText);
                    XmlNodeList xnPage = xmldoc.SelectNodes("/root/manifest/item [@id ='" + Title + "']");
                    var pageval = xnPage[0].Attributes["href"].InnerText;
                    var page = new ReaderBookPage
                    {
                        Title = Title,
                        Path = rootpath + "\\" + pageval,
                        DirPath = rootpath,
                        Index = i,

                    };
                    if (File.Exists(rootpath + "\\statics.xml"))
                    {
                        XmlDocument xmldocstatics = new XmlDocument();
                        xmldocstatics.Load(rootpath + "\\statics.xml");
                        var bookmarknode = xmldocstatics.SelectNodes("/root/bookmarks/bookmark[@currentpage='" + i + "']");
                        if (bookmarknode != null && bookmarknode.Count > 0)
                        {

                            page.BookMarked = true;
                        }
                        else
                            page.BookMarked = false;
                    }
                    else
                        page.BookMarked = false;
                    i++;
                    pages.Add(page);
                }
            }
            catch (Exception ex)
            {
                pages = null;
            }
            return pages;
        }
      
        public IList<ReaderBookPage> ReadXMLFrom(UserReaderBookItem book, string xmlFile, string rootpath)
        {
            List<ReaderBookPage> pages = new List<ReaderBookPage>();
            try
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(xmlFile);
                XmlNodeList xnList = xmldoc.SelectNodes("/root/spine/itemref");
                var dctitle = xmldoc.SelectSingleNode("/root/metadata/title").InnerXml;
                var dccreator = xmldoc.SelectSingleNode("/root/metadata/creator").InnerXml;
                var dcpublisher = xmldoc.SelectSingleNode("/root/metadata/publisher").InnerXml;
                var dcidentifier = xmldoc.SelectSingleNode("/root/metadata/identifier").InnerXml;
                var dclanguage = xmldoc.SelectSingleNode("/root/metadata/language").InnerXml;
                var dcdate = xmldoc.SelectSingleNode("/root/metadata/date").InnerXml;
                var dtmcreate = xmldoc.SelectSingleNode("/root/metadata/dtmcreate").InnerXml;

                book.dccreator = dccreator;
                book.dcpublisher = dcpublisher;
                book.dcidentifier = dcidentifier;
                book.dclanguage = dclanguage;
                book.dcdate = dcdate;
                book.dtmCreate = Common.ToSafeDate(dtmcreate);

                int i = 0;
                foreach (XmlNode x in xnList)
                {
                    var Title = Common.ToSafeString(x.Attributes["idref"].InnerText);
                    XmlNodeList xnPage = xmldoc.SelectNodes("/root/manifest/item [@id ='" + Title + "']");
                    var pageval = xnPage[0].Attributes["href"].InnerText;
                    var page = new ReaderBookPage
                    {
                        Title = Title,
                        Path = rootpath + "\\" + pageval,
                        DirPath = rootpath,
                        Index = i,

                    };
                    if (File.Exists(rootpath + "\\statics.xml"))
                    {
                        XmlDocument xmldocstatics = new XmlDocument();
                        xmldocstatics.Load(rootpath + "\\statics.xml");
                        var bookmarknode = xmldocstatics.SelectNodes("/root/bookmarks/bookmark[@currentpage='" + i + "']");
                        if (bookmarknode != null && bookmarknode.Count > 0)
                        {

                            page.BookMarked = true;
                        }
                        else
                            page.BookMarked = false;
                    }
                    else
                        page.BookMarked = false;
                    i++;
                    pages.Add(page);
                }
            }
            catch (Exception ex)
            {
                pages = null;
            }
            return pages;
        }
        public IList<ReaderBookIndex> ReadIndexFromXML(out string IndexTitle, string rootpath)
        {
            List<ReaderBookIndex> Indexes = new List<ReaderBookIndex>();
            IndexTitle = "";
            try
            {
                var xmlFile = "";
             
                foreach (string indexfile in Directory.GetFiles(rootpath, "*.ncx", SearchOption.AllDirectories))
                {

                    StreamReader indexreader = new StreamReader(indexfile);
                    string indexstEn = indexreader.ReadToEnd();
                    indexreader.Close();
                    var navigationindex = indexstEn.Substring(indexstEn.IndexOf("<docTitle>"), indexstEn.IndexOf("</ncx>") - indexstEn.IndexOf("<docTitle>")).Replace("</ncx>", "");

                    StringBuilder sbindex = new StringBuilder(500);
                    sbindex.Append("<?xml version='1.0' encoding='UTF-8' ?>" + Environment.NewLine);
                    sbindex.Append("<root>" + Environment.NewLine);
                    sbindex.Append(navigationindex + Environment.NewLine);
                    sbindex.Append("</root>" + Environment.NewLine);
                    string stxmlindex = sbindex.ToString();
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.LoadXml(stxmlindex);
                    IndexTitle = xmldoc.SelectNodes("/root/docTitle/text")[0].InnerText;
                    XmlNodeList xnList = xmldoc.SelectNodes("/root/navMap/navPoint");
                    foreach (XmlNode x in xnList)
                    {
                        var Index = Common.ToSafeInt(x.Attributes["playOrder"].InnerText);
                        var Title = x.SelectNodes("navLabel/text")[0].InnerText;
                        var Path = rootpath + "\\" + x.SelectNodes("content")[0].Attributes["src"].InnerText;
                        var navpoint = new ReaderBookIndex
                        {
                            Title = Title,
                            Path = Path,
                            DirPath = rootpath,
                            Index = Index,
                        };
                        navpoint.Indexes = ReadRecuursive(rootpath, x);
                        Indexes.Add(navpoint);
                    }
                }

              
            }
            catch (Exception ex)
            {
                string st = ex.Message;
                Indexes = null;
            }
            return Indexes;

        }
        public string SetCoverPage(string rootpath)
        {
            string page = "";
            try
            {
                var path = rootpath + "\\images\\cover.jpg";
                if (File.Exists(path))
                {
                    page = path;
                }
                else
                {
                    //page = Constants.AppImage() + "blankbook.png";
                }
            }
            catch
            {
                //  page = Constants.AppImage() + "blankbook.png";

            }
            return page;
        }
        public IList<objBookMark> ReadBookMark(string BookStaticsPath)
        {
            List<objBookMark> Bookmarks = new List<objBookMark>();
            if (File.Exists(BookStaticsPath))
            {
                StringBuilder sb = new StringBuilder(500);
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(BookStaticsPath);

                var bookmarknode = xmldoc.SelectNodes("/root/bookmarks/bookmark");
                foreach (XmlNode item in bookmarknode)
                {
                    var Index = Common.ToSafeInt(item.Attributes["currentpage"].InnerText);
                    var Title = Common.ToSafeString(item.Attributes["title"].InnerText);
                    var date = Common.ToSafeString(item.Attributes["date"].InnerText);
                    var bookmark = new objBookMark
                    {
                        Title = Title,
                        Index = Index,
                        dtmCreate = date,
                    };
                    Bookmarks.Add(bookmark);
                }
                return Bookmarks.OrderBy(m => m.Index).ToList();
            }
            else
            {
                return null;
            }

        }
        public IList<objNote> ReadBookNotes(string BookStaticsPath)
        {
            List<objNote> notes = new List<objNote>();
            if (File.Exists(BookStaticsPath))
            {
                StringBuilder sb = new StringBuilder(500);
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(BookStaticsPath);

                var notesnode = xmldoc.SelectNodes("/root/notes/note");
                foreach (XmlNode item in notesnode)
                {
                    var currentpage = Common.ToSafeInt(item.Attributes["currentpage"].InnerText);
                    var title = Common.ToSafeString(item.Attributes["title"].InnerText);
                    var date = Common.ToSafeString(item.Attributes["date"].InnerText);
                    var selectedtext = Common.ToSafeString(item.Attributes["selectedtext"].InnerText);
                    var notetext = Common.ToSafeString(item.Attributes["notetext"].InnerText);
                    var key = Common.ToSafeString(item.Attributes["key"].InnerText);

                    var note = new objNote
                    {
                        Title = title + "\n" + date,
                        currentindex = currentpage,
                        notetext = notetext,
                        selectedtext = selectedtext,
                        key = key,
                        dtmUpdate = date,
                    };
                    notes.Add(note);
                }
                return notes.OrderBy(m => m.currentindex).ToList();
            }
            else
            {
                return null;
            }

        }
        public void CreateBookMark(string BookStaticsPath, int currentindex, string Title)
        {
            if (File.Exists(BookStaticsPath))
            {
                StringBuilder sb = new StringBuilder(500);
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(BookStaticsPath);
                var bookmarknode = xmldoc.SelectNodes("/root/bookmarks/bookmark[@currentpage='" + currentindex + "']");
                if (bookmarknode == null || bookmarknode.Count == 0)
                {
                    XmlElement bookmark = xmldoc.CreateElement("bookmark");
                    bookmark.SetAttribute("currentpage", currentindex.ToString());
                    bookmark.SetAttribute("title", Title);
                    bookmark.SetAttribute("date", DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString() + " : " + DateTime.Now.Hour.ToString() + " : " + DateTime.Now.Minute.ToString() + " : " + DateTime.Now.Second.ToString());
                    XmlNode NodeItem = xmldoc.SelectSingleNode("/root/bookmarks");
                    NodeItem.AppendChild(bookmark);
                    xmldoc.Save(BookStaticsPath);
                }

            }
        }
        public string SetLastPage(string rootpath)
        {
            string page = "";
            try
            {
                var path = rootpath + "\\images\\back.jpg";
                if (File.Exists(path))
                {
                    page = path;
                }
                else { }
                //page = Constants.AppImage() + "blankbook.png";
            }
            catch
            {
                //page = Constants.AppImage() + "blankbook.png";

            }
            return page;
        }
        public IList<ReaderBookPage> Pages(XmlDocument xmldoc, string rootpath)
        {
            List<ReaderBookPage> pages = new List<ReaderBookPage>();
            try
            {
                int i = 1;
                XmlNodeList xnList = xmldoc.SelectNodes("/root/spine/itemref");
                foreach (XmlNode x in xnList)
                {
                    var Title = Common.ToSafeString(x.Attributes["idref"].InnerText);
                    XmlNodeList xnPage = xmldoc.SelectNodes("/root/manifest/item [@id ='" + Title + "']");
                    var pageval = xnPage[0].Attributes["href"].InnerText;
                    var page = new ReaderBookPage
                    {
                        Title = (Title.Contains("p") == true) ? i.ToString() : Title,
                        Path = rootpath + "\\" + pageval,
                        DirPath = rootpath,
                        Index = i,
                    };

                    i++;
                    pages.Add(page);
                }
            }
            catch (Exception ex)
            {
                pages = null;
            }
            return pages;
        }
        public IList<ReaderBookIndex> ReadRecuursive(string rootpath, XmlNode xn)
        {
            List<ReaderBookIndex> Indexes = new List<ReaderBookIndex>();
            XmlNodeList xnList = xn.SelectNodes("navPoint");

            if (xnList == null && xnList.Count == 0)
            {
                return Indexes;

            }
            else
            {
                foreach (XmlNode x in xnList)
                {
                    var Index = Common.ToSafeInt(x.Attributes["playOrder"].InnerText);
                    var Title = x.SelectNodes("navLabel/text")[0].InnerText;
                    var Path = rootpath + "\\" + x.SelectNodes("content")[0].Attributes["src"].InnerText;
                    var navpoint = new ReaderBookIndex
                    {
                        Title = Title,
                        Path = Path,
                        DirPath = rootpath,
                        Index = Index,

                    };
                    navpoint.Indexes = ReadRecuursive(rootpath, x);
                    Indexes.Add(navpoint);
                }
            }
            return Indexes;

        }
        public void BookStaticCreate(string BookStaticsPath, int firstIdex, int FontSize, string BgColor, string FontColor, double BrightNess, string zoomin)
        {
            if (!File.Exists(BookStaticsPath))
            {
                StringBuilder sb = new StringBuilder(500);
                sb.Append("<?xml version='1.0' encoding='UTF-8' ?>" + Environment.NewLine);
                sb.Append("<root>" + Environment.NewLine);
                sb.Append("<lastreadpage index='" + firstIdex + "' ></lastreadpage>");
                sb.Append("<fontsize size='" + FontSize + "' ></fontsize>");
                sb.Append("<bgcolor color='" + BgColor + "' ></bgcolor>");
                sb.Append("<fontcolor color='" + FontColor + "' ></fontcolor>");
                sb.Append("<brightness bright='" + BrightNess + "' ></brightness>");
                sb.Append("<zoomin zoom='" + zoomin + "' ></zoomin>");
                sb.Append("<bookmarks></bookmarks>");
                sb.Append("<notes></notes>");


                sb.Append("</root>" + Environment.NewLine);
                string stxml = sb.ToString();
                StreamWriter writere = new StreamWriter(BookStaticsPath);
                writere.Write(stxml);
                writere.Close();
                Thread.Sleep(5000);
            }
        }
        public void BookStaticUpdate(string BookStaticsPath, int firstIdex, int FontSize, string BgColor, string FontColor, double BrightNess, string zoomin)
        {
            if (File.Exists(BookStaticsPath))
            {
                StringBuilder sb = new StringBuilder(500);
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(BookStaticsPath);
                xmldoc.SelectNodes("/root/lastreadpage")[0].Attributes["index"].InnerText = firstIdex.ToString();
                xmldoc.SelectNodes("/root/fontsize")[0].Attributes["size"].InnerText = FontSize.ToString();
                xmldoc.SelectNodes("/root/bgcolor")[0].Attributes["color"].InnerText = BgColor;
                xmldoc.SelectNodes("/root/fontcolor")[0].Attributes["color"].InnerText = FontColor;
                xmldoc.SelectNodes("/root/brightness")[0].Attributes["bright"].InnerText = BrightNess.ToString();
                xmldoc.SelectNodes("/root/zoomin")[0].Attributes["zoom"].InnerText = zoomin;
                xmldoc.Save(BookStaticsPath);
            }
        }
        public void BookStaticRead(string BookStaticsPath, out int currentindex, out int FontSize, out string BgColor,
            out string FontColor, out double BrightNess, out string zoomin)
        {
            StringBuilder sb = new StringBuilder(500);
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(BookStaticsPath);
            currentindex = Common.ToSafeInt(xmldoc.SelectNodes("/root/lastreadpage")[0].Attributes["index"].InnerText);
            FontSize = Common.ToSafeInt(xmldoc.SelectNodes("/root/fontsize")[0].Attributes["size"].InnerText);
            BgColor = xmldoc.SelectNodes("/root/bgcolor")[0].Attributes["color"].InnerText;
            FontColor = xmldoc.SelectNodes("/root/fontcolor")[0].Attributes["color"].InnerText;
            BrightNess = Common.ToSafeDouble(xmldoc.SelectNodes("/root/brightness")[0].Attributes["bright"].InnerText);

            zoomin = xmldoc.SelectNodes("/root/zoomin")[0].Attributes["zoom"].InnerText;

        }

        public static class Encryption
        {
            //private static byte[] _salt = Encoding.ASCII.GetBytes("b9eafac5-27f5-446a-b8fa-f1a9f9b3788e");
            //private static string key = "beb65e7b-1daa-4f8c-868a-0cffb08dfa23";

            private static byte[] _salt = Encoding.ASCII.GetBytes("b9eafac5-27f5-446a-b8fa-f1a9f9b3788e");
            private static string key = "beb65e7b-1daa-4f8c-868a-0cffb08dfa23";
            public static string DecryptStringDES(string cipherText, string _Key)
            {
                string sharedSecret = _Key;
                if (string.IsNullOrEmpty(cipherText))
                    throw new ArgumentNullException("cipherText");
                if (string.IsNullOrEmpty(sharedSecret))
                    throw new ArgumentNullException("sharedSecret");
                RijndaelManaged aesAlg = null;
                string plaintext = "";
                try
                {
                    Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, _salt);
                    aesAlg = new RijndaelManaged();
                    aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                    aesAlg.IV = key.GetBytes(aesAlg.BlockSize / 8);
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    byte[] bytes = Convert.FromBase64String(cipherText);
                    using (MemoryStream msDecrypt = new MemoryStream(bytes))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                                plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
                finally
                {
                    if (aesAlg != null)
                        aesAlg.Clear();
                }
                return plaintext;
            }
            public static string EncryptStringAES(string plainText, string sharedSecret)
            {
                if (string.IsNullOrEmpty(plainText))
                    throw new ArgumentNullException("plainText");
                if (string.IsNullOrEmpty(sharedSecret))
                    throw new ArgumentNullException("sharedSecret");
                string outStr = null;                       // Encrypted string to return 
                RijndaelManaged aesAlg = null;              // RijndaelManaged object used to encrypt the data. 
                try
                {
                    Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, _salt);
                    aesAlg = new RijndaelManaged();
                    aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                    aesAlg.IV = key.GetBytes(aesAlg.BlockSize / 8);
                    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(plainText);
                            }
                        }
                        outStr = Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
                finally
                {
                    if (aesAlg != null)
                        aesAlg.Clear();
                }
                return outStr;
            }
            public static string StringAES(string plainText)
            {
                if (plainText == String.Empty)
                {
                    return "";
                }
                if (plainText == null)
                {
                    return "";
                }
                string sharedSecret = key;
                if (string.IsNullOrEmpty(plainText))
                    throw new ArgumentNullException("plainText");
                if (string.IsNullOrEmpty(sharedSecret))
                    throw new ArgumentNullException("sharedSecret");

                string outStr = null;                       // Encrypted string to return 
                RijndaelManaged aesAlg = null;              // RijndaelManaged object used to encrypt the data. 

                try
                {
                    Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, _salt);
                    aesAlg = new RijndaelManaged();
                    aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                    aesAlg.IV = key.GetBytes(aesAlg.BlockSize / 8);
                    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(plainText);
                            }
                        }
                        outStr = Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
                finally
                {
                    if (aesAlg != null)
                        aesAlg.Clear();
                }
                return outStr;
            }
            public static string StringDES(string cipherText)
            {
                if (cipherText == String.Empty)
                {
                    return "";
                }
                string sharedSecret = key;
                if (string.IsNullOrEmpty(cipherText))
                    throw new ArgumentNullException("cipherText");
                if (string.IsNullOrEmpty(sharedSecret))
                    throw new ArgumentNullException("sharedSecret");
                RijndaelManaged aesAlg = null;
                string plaintext = null;
                try
                {
                    Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, _salt);
                    aesAlg = new RijndaelManaged();
                    aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                    aesAlg.IV = key.GetBytes(aesAlg.BlockSize / 8);
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    byte[] bytes = Convert.FromBase64String(cipherText);
                    using (MemoryStream msDecrypt = new MemoryStream(bytes))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                                plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
                finally
                {
                    if (aesAlg != null)
                        aesAlg.Clear();
                }

                return plaintext;
            }
            public static string EncryptCommon(string textToEncrypt, string _key = "")
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(textToEncrypt))
                    {
                        return "";
                    }
                    RijndaelManaged rijndaelCipher = new RijndaelManaged();
                    rijndaelCipher.Mode = CipherMode.CBC;
                    rijndaelCipher.Padding = PaddingMode.PKCS7;
                    key = _key == "" ? key : _key;
                    rijndaelCipher.KeySize = 0x80; // 256bit key
                    rijndaelCipher.BlockSize = 0x80;
                    byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
                    byte[] keyBytes = new byte[0x10];
                    int len = pwdBytes.Length;
                    if (len > keyBytes.Length)
                    {
                        len = keyBytes.Length;
                    }
                    Array.Copy(pwdBytes, keyBytes, len);
                    rijndaelCipher.Key = keyBytes;
                    rijndaelCipher.IV = keyBytes;
                    ICryptoTransform transform = rijndaelCipher.CreateEncryptor();
                    byte[] plainText = Encoding.UTF8.GetBytes(textToEncrypt);
                    return Convert.ToBase64String(transform.TransformFinalBlock(plainText, 0, plainText.Length));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            public static string DecryptCommon(string textToDecrypt, string _key = "")
            {
                if (string.IsNullOrWhiteSpace(textToDecrypt))
                {
                    return "";
                }
                key = _key == "" ? key : _key;
                RijndaelManaged rijndaelCipher = new RijndaelManaged();
                rijndaelCipher.Mode = CipherMode.CBC;
                rijndaelCipher.Padding = PaddingMode.PKCS7;

                rijndaelCipher.KeySize = 0x80;
                rijndaelCipher.BlockSize = 0x80;
                byte[] encryptedData = Convert.FromBase64String(textToDecrypt);
                byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
                byte[] keyBytes = new byte[0x10];
                int len = pwdBytes.Length;
                if (len > keyBytes.Length)
                {
                    len = keyBytes.Length;
                }
                Array.Copy(pwdBytes, keyBytes, len);
                rijndaelCipher.Key = keyBytes;
                rijndaelCipher.IV = keyBytes;
                byte[] plainText = rijndaelCipher.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);
                return Encoding.UTF8.GetString(plainText);
            }

            public static void DecryptFile(string inputFile, string outputFile, string _Key)
            {
               
                    string sharedSecret = _Key; ;

                    UnicodeEncoding UE = new UnicodeEncoding();
                    // byte[] key = UE.GetBytes(sharedSecret);
                    byte[] _salt = Encoding.ASCII.GetBytes("b9eafac5-27f5-446a-b8fa-f1a9f9b3788e");
                    FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);

                    RijndaelManaged RMCrypto = new RijndaelManaged();
                    Rfc2898DeriveBytes rfckey = new Rfc2898DeriveBytes(key, _salt);

                    RMCrypto.Key = rfckey.GetBytes(RMCrypto.KeySize / 8);
                    RMCrypto.IV = rfckey.GetBytes(RMCrypto.BlockSize / 8);

                    //CryptoStream cs = new CryptoStream(fsCrypt,RMCrypto.CreateDecryptor(key, key),CryptoStreamMode.Read);
                    //when using Salt
                    CryptoStream cs = new CryptoStream(fsCrypt, RMCrypto.CreateDecryptor(RMCrypto.Key, RMCrypto.IV), CryptoStreamMode.Read);
                    FileStream fsOut = new FileStream(outputFile, FileMode.Create);

                    int data;
                    while ((data = cs.ReadByte()) != -1)
                        fsOut.WriteByte((byte)data);

                    fsOut.Close();
                    cs.Close();
                    fsCrypt.Close();

               

            }

        }

    }
}
