using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ReadEdgeCore.Models.Interfaces;
using ReadEdgeCore.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace ReadEdgeCore.Models
{
    public class EbookReader : IEbookReader
    {
        #region Props
        private IReaderBookService _readerBookService;
        private static IHostingEnvironment _hostingEnvironment;
        private IHttpContextAccessor _HttpContextAccessor;
        private IReaderBooks _readerBooks;
        string dvKey = "";
        int BrightNess = 100;
        string BgColor = "#forestgreen";
        string FontColor = "#000000";
        string rootPath = "/Books/";
        static string folderPath = "Prachi - New Edgeways Multiskill English MCB - 1/";
        string encKey = "";
        string sub = "OEBPS/";
        static int lastindex = 0;
        const string SessionName = "_Name";
        #endregion
        #region Constructor
        public EbookReader(IHostingEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor, IReaderBookService readerBookService, IReaderBooks readerBooks)
        {
            _readerBookService = readerBookService;
            _hostingEnvironment = hostingEnvironment;
            _readerBooks = readerBooks;
            _HttpContextAccessor = httpContextAccessor;
            _HttpContextAccessor = httpContextAccessor;
            _HttpContextAccessor.HttpContext.Session.SetInt32("currentindex", 1);
        }

        #endregion
        #region Methods
        public string OpenEbook()
        {

            //readerBooks = GetReaderBooks();
            //this.Title = readerBooks.Title;
            //BindTree();
            return LoadHtml();
            //lblName.Text = readerBooks.Title;
        }
        string LoadHtml()
        {
            //var filename = "p005.xhtml";
            var filename = "cover.xhtml";

            var dire = rootPath + folderPath + "\\" + sub;
            string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Books");
            string filePath = Path.Combine(uploadsFolder, folderPath + "\\" + sub + filename);
            var text = System.IO.File.ReadAllText(filePath);
            return GetCurrentPageHtmlString(dire);
        }
        public ReaderBooks GetReaderBooks(string fileName, string EncKey)
        {
            folderPath = fileName + "/" + fileName;
            encKey = EncKey;
            string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Books");
            string bookpath = Path.Combine(uploadsFolder, folderPath);
            //var bookpath = HttpContext.Current.Server.MapPath("~/Books/" + folderPath);
            string IndexTitle;
            var readerBookPages = new List<ReaderBookPage>();
            foreach (string fl in Directory.GetFiles(bookpath, "*.opf", SearchOption.AllDirectories))
            {
                string line;
                var reader = new StreamReader(fl);
                line = reader.ReadToEnd();
                reader.Close();
                //var encKey = "5dd0702c-51b4-483d-9cf8-2db23dc60992";
                var stEn = line;
                if (!string.IsNullOrEmpty(encKey))
                {
                    stEn = line = Cryptography.DecryptCommon(line, encKey);
                }

                var metadata = stEn.Substring(stEn.IndexOf("<metadata"), stEn.IndexOf("</metadata>") - stEn.IndexOf("<metadata")) + "<dtmcreate>" + DateTime.Now.ToString() + "</dtmcreate></metadata>";
                metadata = "<metadata" + metadata.Substring(metadata.IndexOf('>'), metadata.Length - metadata.IndexOf('>'));
                metadata = metadata.Replace("dc:", "dc");
                metadata = metadata.Replace(":dc", "dc");

                var pagename = stEn.Substring(stEn.IndexOf("<manifest>"), stEn.IndexOf("</manifest>") - stEn.IndexOf("<manifest>")) + "</manifest>";
                var navigation = stEn.Substring(stEn.IndexOf("<spine toc=\"ncx\">"), stEn.IndexOf("</spine>") - stEn.IndexOf("<spine toc=\"ncx\">")) + "</spine>";

                var sb = new StringBuilder(500);
                sb.Append("<?xml version='1.0' encoding='UTF-8' ?>" + Environment.NewLine);
                sb.Append("<root>" + Environment.NewLine);
                sb.Append(metadata + Environment.NewLine + pagename + Environment.NewLine + navigation + Environment.NewLine);
                sb.Append("</root>" + Environment.NewLine);
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(sb.ToString());

                XmlNodeList xnList = xmldoc.SelectNodes("/root/spine/itemref");
                string Title = xmldoc.SelectSingleNode("/root/metadata/dctitle").InnerXml;
                _readerBooks.Title = Title;
                _readerBooks.DirPath = bookpath + "\\" + "OEBPS";

                _readerBooks.dccreator = xmldoc.SelectSingleNode("/root/metadata/dccreator").InnerXml;
                if (xmldoc.SelectSingleNode("/root/metadata/dcpublisher") != null)
                {
                    _readerBooks.dcpublisher = xmldoc.SelectSingleNode("/root/metadata/dcpublisher").InnerXml;
                }
                else
                {
                    _readerBooks.dcpublisher = string.Empty;
                }
                if (xmldoc.SelectSingleNode("/root/metadata/dcidentifier") != null)
                {
                    _readerBooks.dcidentifier = xmldoc.SelectSingleNode("/root/metadata/dcidentifier").InnerXml;
                }
                else
                {
                    _readerBooks.dcidentifier = string.Empty;
                }

                if (xmldoc.SelectSingleNode("/root/metadata/dclanguage") != null)
                {
                    _readerBooks.dclanguage = xmldoc.SelectSingleNode("/root/metadata/dclanguage").InnerXml;
                }
                else
                {
                    _readerBooks.dclanguage = string.Empty;
                }
                //readerBooks.dclanguage = xmldoc.SelectSingleNode("/root/metadata/dclanguage").InnerXml;
                if (xmldoc.SelectSingleNode("/root/metadata/dcdate") != null)
                {
                    _readerBooks.dcdate = xmldoc.SelectSingleNode("/root/metadata/dcdate").InnerXml;
                }
                else
                {
                    _readerBooks.dcdate = string.Empty;
                }
                //readerBooks.dcdate = xmldoc.SelectSingleNode("/root/metadata/dcdate").InnerXml;
                _readerBooks.Pages = _readerBookService.Pages(xmldoc, _readerBooks.DirPath);
                _readerBooks.CoverPage = _readerBookService.SetCoverPage(_readerBooks.DirPath);
                _readerBooks.LastPage = _readerBookService.SetLastPage(_readerBooks.DirPath);
                _readerBooks.DirPath = _readerBooks.DirPath;
                _readerBooks.BookPath = bookpath + _readerBooks.Title;
                _readerBooks.Indexes = _readerBookService.ReadIndexFromXML(out IndexTitle, _readerBooks.DirPath);
                _readerBooks.IndexTitle = _readerBooks.Title;
                _readerBooks.Title = _readerBooks.Title;
                _readerBooks.Downloaded = true;
                _readerBooks.DownloadPercentage = 100;
                _readerBooks.BookPath = bookpath;
                _readerBooks.EpubPath = "";
                _readerBooks.EncriptionKey = encKey;
                return (ReaderBooks)_readerBooks;
            }
            return null;
        }

        string GetCurrentPageHtmlString(string dire)
        {
            var currentindex = _HttpContextAccessor.HttpContext.Session.GetInt32("currentindex");
            //var currentindex = HttpContext.Current.Session["currentindex"];
            int index = 1;
            if (currentindex != null || currentindex.ToString() == "")
            {
                index = Convert.ToInt32(currentindex);
            }

            //this.Title = readerBooks.Title;
            var pagename = Common.readerBooks.Pages[index - 1].Path;
            lastindex = Common.readerBooks.Pages.Count();
            return GetHtml(dire, pagename);
        }

        private string GetHtml(string dire, string pagename)
        {
            _HttpContextAccessor.HttpContext.Session.Remove("Audio");
            //HttpContext.Current.Session.Remove("Audio");
            _HttpContextAccessor.HttpContext.Session.SetInt32("Audio", 0);
            //HttpContext.Current.Session["Audio"] = false;
            StreamReader reader = new StreamReader(pagename);
            var content = reader.ReadToEnd();
            //string stEn = reader.ReadToEnd();
            reader.Close();
            string stEn = string.Empty;
            if (!string.IsNullOrEmpty(Common.readerBooks.EncriptionKey))
            {
                stEn = Cryptography.DecryptCommon(content, Common.readerBooks.EncriptionKey);
            }
            else
            {
                stEn = content;
            }
            var x = @"<?xml version=""1.0"" encoding=""UTF-8""?>";
            stEn = stEn.Replace(x, "");
            //var bootstrapPath = HttpContext.Current.Server.MapPath("~/Content/Bootstrap/");
            //var bootstrapPath = "/Content/Bootstrap/";
            // string bootstrapPath = Path.Combine(_hostingEnvironment.WebRootPath, "js");
            string bootstrapPath = "/js/";
            var mediaStream = string.Empty;
            var width = "";
            var height = "";
            try
            {
                if (stEn.IndexOf("content=\"width") > 0)
                {
                    var index = stEn.IndexOf("content=\"width") + 9;
                    string amountString = stEn.Substring(index, 28);
                    var arrays = amountString.Split(',');
                    width = Regex.Replace(arrays[0].Split('=')[1], @"[^0-9.]+", "") + "px";
                    height = Regex.Replace(arrays[1].Split('=')[1], @"[^0-9.]+", "") + "px";
                }
            }
            catch
            {
                width = "1215px";
                height = "845px";
            }


            stEn = stEn.Replace("-webkit-", "-ms-");
            if (stEn.IndexOf("bootstrap.min.js") < 0)
            {
                stEn = stEn.Replace("<head>", "<head><script  src = '" + bootstrapPath + "bootstrap.min.js' ></script>");
            }
            if (stEn.IndexOf("jquery-2.2.1.min.js") < 0)
            {
                stEn = stEn.Replace("<head>", "<head><script  src = '" + bootstrapPath + "jquery-2.2.1.min.js' ></script>");
            }
            //stEn = stEn.Replace(" href=\"", " onclick='window.external.HyperLinkClicked(this.href)' href=\"" + dire + "\\");
            stEn = stEn.Replace(" href=\"", " onclick='GetHtml(this.href); return false;'  href=\"" + dire + "\\");
            stEn = stEn.Replace(" src=\"", " src=\"" + dire + "\\");
            if (stEn.IndexOf("oncontextmenu") < 0)
            {
                stEn = stEn.Replace("<body", "<div id='subtitles' oncontextmenu = 'return false' style='position: relative !important;margin: 0 auto !important;margin-bottom: 5% !important;left:0;top:0;'><div style='width:" + width + ";height:" + height + ";position: relative !important;margin: 0 auto !important;overflow:hidden;'");
            }

            var brh = 90;// slBrightNess.Value;
            BrightNess = 100 - brh;
            BrightNess = BrightNess == 0 ? 0 : BrightNess / 100;
            if (BgColor == "#000000")
            {
                FontColor = "#6B2E0F";
            }
            else
            {
                FontColor = "#000000";
            }

            stEn = stEn.Replace("</body>", "</div></div>");


            var text = string.Empty;
            if (stEn.Contains("id='audioPlayer1' src='"))
            {
                _HttpContextAccessor.HttpContext.Session.SetInt32("Audio", 1);
                //HttpContext.Current.Session["Audio"] = true;
                var audioPath = "id='audioPlayer1' src='" + dire + "\\";
                stEn = stEn.Replace("id='audioPlayer1' src='", audioPath);

                //stEn = stEn.Replace("<div style='display: none' class='buttons-rw' id='buttons'>", "<div style='display: block;position:fixed;bottom:5px;right:14%; height:30px;z-index:99999;' class='buttons-rw' id='buttons'>");
                //stEn = stEn.Replace("<button class='play' id='play1' type='button'></button><button class='stop' id='stop1' type='button'></button>", "<button class='play' id='play1' type='button' style='background-color:#337ab7;border-color: #2e6da4;color: #fff;padding:5px;'>Play / Pause</button>&nbsp;<button class='stop' id='stop1' type='button' style='background-color:#ac2925;border-color: #761c19;;color: #fff;padding:5px;'>Stop</button>");

                var filename = System.IO.Path.GetFileName(pagename);
                var ext = System.IO.Path.GetExtension(pagename);
                var audioFilename = dire + "\\audio\\" + filename.Replace(ext, ".mp3");
                var smileFile = dire + "\\smil\\" + filename.Replace(ext, ".smil");
                try
                {
                    mediaStream = AudioStream(smileFile);
                }
                catch (Exception ex) { }
                text = GetAudioScript(mediaStream);
            }

            if (stEn.IndexOf("oncontextmenu") < 0)
            {
                stEn = stEn.Replace("<body", "<body oncopy='return false' oncut='return false' onpaste='return false' id='subtitles' oncontextmenu = 'return false' style='position: relative !important;margin: 0 auto !important;left:0;top:0;'><div style='width:" + width + ";height:" + height + ";position: relative !important;margin: 0 auto !important;overflow:hidden;'");
            }

            if (stEn.IndexOf("black_overlay") < 0)
            {
                stEn = stEn.Replace("</body>", "<div class='black_overlay'></div></body>");
            }

            if (BgColor == "#000000")
            {
                FontColor = "#6B2E0F";
            }
            else
            {
                FontColor = "#000000";
            }

            stEn = stEn.Replace("</div></body>", "");
            stEn = stEn.Replace("</html>", "");

            if (stEn.IndexOf("<style>") > 0)
            {
                var s1 = stEn.Substring(0, stEn.IndexOf("<style>"));
                stEn = s1;
            }

            stEn = stEn + "<style> .cssAddNote{border-bottom: 2px dashed #c22a2a;text-decoration: none;} .jsAddNote {z-index:1001;background-color:blue; background: url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAUCAYAAACEYr13AAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAEZJREFUeNpiYBjsYD8Ukw3+QzFOwESpE4eBASxALADEBgTUOeAQvwCT/E8mdgC54AEQN+KwoR5K45J/MJoOBoMBwwAABBgAl1cYmFF1UkIAAAAASUVORK5CYII=') no-repeat  center; padding: 8px 10px 8px 10px; }.jsremove {z-index:1001;background-color:blue;  background:  url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAUCAYAAACEYr13AAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAARVJREFUeNrMU9ERwUAQjQzfdJDoQAeSCujA6IAKQgc6UAIqyKWDUwE6iG8zeDvzbuZsjvhjZ9bt7u29fXY3UfRr6cjPIe6lOApoyngF3Uzut7oNIMbjKR9foGfoiH6Ju4FLhL2DjoIMfGFSCZXHRkDBZI74iXb+wkADIMHi2NPNvKu5+ABafARAQqYeHghsCFwg5+GAQgwMdMiKFvZesXAyfukBG7b4YnJ95uVSrOtduO63Sc7cLXQY+9RproW6Z0f0nS2jXsrOgPWqq9Brdeq4FDrzLwtgogGkUsJN9CdRMW49oFVoCrW3zlrSALMGwJEbaAPMQvEGgPsWQj2Q+PUbAJEdz5nyjQYINdG8AdX2n8hTgAEAsipfqAIBnqEAAAAASUVORK5CYII=') no-repeat  center; padding: 8px 10px 8px 10px; }.cssnote {background-color:lightgray;height:auto; position: absolute; z-index: 999;text-align: center;margin:-20px 0px 0px 0px;} .highlight { background-color: #0ffafa;  color: #000; }body{z-index:100;  background-color:" + BgColor + ";color:" + FontColor + ";}";

            if (stEn.IndexOf("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">") < 0)
            {
                stEn = stEn.Replace("<head>", "<head> <meta http-equiv= 'Content-Type' content ='text/html; charset=utf-8' /> ");
            }

            if (BgColor == "#000000")
            {
                stEn = stEn + " a{color:#c9e7f9 } ";
            }
            if (BrightNess > 0)
            {
                stEn = stEn.Replace(".black_overlay{position: fixed; z-index:-1; top:0%; left: 0%; width: 100%; height: 100%; background-color: black; -moz-opacity:" + BrightNess + "; opacity: " + BrightNess + "; filter: alpha(opacity = " + BrightNess * 10 + ");}", "");

                stEn = stEn + ".black_overlay{position: fixed; z-index:-1; top: 0%; left: 0%; width: 100%; height: 100%; background-color: black; -moz-opacity:" + BrightNess + "; opacity: " + BrightNess + "; filter: alpha(opacity = " + BrightNess * 10 + ");}";
            }
            stEn = stEn + "</style></div></body>";

            if (stEn.IndexOf("jreader.1.1.js") < 0)
            {

                stEn = stEn.Replace("</body>", "<script  src = '" + bootstrapPath + "jreader.1.1.js'></script></body>");
            }


            stEn = stEn.Replace("</body>", text + "</div></body>");
            if (!string.IsNullOrWhiteSpace(dvKey))
            {
                stEn += "<script>SetFocus('" + dvKey + "')</script></html>";
            }
            else
            {
                stEn = stEn + "</html>";
            }

            return stEn;
        }

        string AudioStream(string file)
        {
            var results = new List<AudioModel>();
            //string path = HttpContext.Current.Server.MapPath("~/");
            string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath);
            string fullpath = Path.Combine(uploadsFolder + file);
            if (File.Exists(fullpath))
            {
                string text;
                var fileStream = new FileStream(fullpath, FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    text = streamReader.ReadToEnd();
                }
                var smiles = Common.ConvertFromXML<Smil>(text);
                if (smiles != null && smiles.Body != null && smiles.Body.Par != null && smiles.Body.Par.Any())
                {
                    foreach (var item in smiles.Body.Par)
                    {
                        var audio = new AudioModel
                        {
                            end = ConvertTomeToSec(item.Audio.ClipEnd).ToString(),
                            start = ConvertTomeToSec(item.Audio.ClipBegin).ToString(),
                            text = item.Text.Src.Split('#')[1]
                        };
                        results.Add(audio);
                    }
                }

            }
            return JsonConvert.SerializeObject(results, Newtonsoft.Json.Formatting.Indented);
        }

        double ConvertTomeToSec(string mnts)
        {
            var allmnts = mnts.Split(':');
            var hrs = Convert.ToDouble(allmnts[0]);
            var mn = Convert.ToDouble(allmnts[1]);
            var sec = Convert.ToDouble(allmnts[2]);
            var seconds = 0D;
            seconds = hrs * 60 * 60;
            seconds = seconds + (mn * 60);
            seconds = seconds + sec;
            return seconds;

        }

        string GetAudioScript(string mediaStream)
        {
            try
            {
                var scriptTag = "<script>";
                scriptTag = scriptTag + "function xxx(){";
                scriptTag = scriptTag + "var i = '';";
                scriptTag = scriptTag + "var audioPlayer = document.getElementById('audioPlayer1');";
                scriptTag = scriptTag + "var subtitles = document.getElementById('subtitles');";
                scriptTag = scriptTag + "audioPlayer.currentTime = 0;audioPlayer.paused= false ;var syncData = [];";
                scriptTag = scriptTag + "var allData = " + mediaStream + ";";


                scriptTag = scriptTag + "$.each(allData, function(i, item) {";
                scriptTag = scriptTag + "var word = document.getElementById(item.text).innerText;";
                scriptTag = scriptTag + "syncData.push({ text: word, start: item.start, end: item.end, ele: item.text });";
                scriptTag = scriptTag + "});";

                scriptTag = scriptTag + "audioPlayer.addEventListener('timeupdate', function(e) {";
                scriptTag = scriptTag + "$.each(syncData, function(inde, dataItem) {";
                scriptTag = scriptTag + "if (audioPlayer.currentTime >= dataItem.start)";
                scriptTag = scriptTag + "{";
                scriptTag = scriptTag + "if (inde >0)";
                scriptTag = scriptTag + "{";
                scriptTag = scriptTag + "document.getElementById(i).style.background = 'transparent';";
                scriptTag = scriptTag + "}";
                scriptTag = scriptTag + "i = dataItem.ele;";
                scriptTag = scriptTag + "document.getElementById(dataItem.ele).style.background = 'yellow';";
                scriptTag = scriptTag + "}";
                scriptTag = scriptTag + "});";
                scriptTag = scriptTag + "});";
                scriptTag = scriptTag + "return audioPlayer;}function updateTime(){ var thisPlayer = $(this);}";
                scriptTag = scriptTag + "$('#play1').click(function() {var audioPlayer=xxx(); var src = $(audioPlayer).attr('src');  if(audioPlayer.paused == false){audioPlayer.pause();}else{audioPlayer.play();audioPlayer.bind('timeupdate', updateTime);  } });";
                scriptTag = scriptTag + "$('#stop1').click(function() {alert('Paused'); audioPlayer.currentTime = 0;if (!audioPlayer.paused) {audioPlayer.pause();}$('.audio').css('background', 'transparent');});";

                scriptTag = scriptTag + "</script>";
                return scriptTag;
            }
            catch (Exception ex) { }

            return "";
        }

        public string LoadPage(int pageID)
        {
            var pageIndex = Common.ToSafeInt(pageID);
            var dire = rootPath + folderPath + "\\" + sub;
            //HttpContext.Current.Session["currentindex"] = pageIndex;
            _HttpContextAccessor.HttpContext.Session.SetInt32("currentindex", pageIndex);
            var result = GetCurrentPageHtmlString(dire);
            return result;
        }

        public string OpenPage(string pageReference)
        {

            string[] stringSeparators = new string[] { "OEBPS//" };
            string[] CurrentPage = pageReference.Split(stringSeparators, StringSplitOptions.None);
            var dire = folderPath + sub;
            //var virtualdir = HttpContext.Current.Server.MapPath("~" + dire);
            string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Books");
            string virtualdir = Path.Combine(uploadsFolder, dire);

            var pagename = virtualdir + "/" + CurrentPage[1];
            var result = GetHtml(dire, pagename);
            return result;
        } 
        #endregion
    }
}

