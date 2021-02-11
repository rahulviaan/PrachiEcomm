using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace PrachiIndia.Portal.Controllers
{
    public class TestController : ApiController
    {
        [HttpPost]

        public HttpResponseMessage ProcFile(FileModel filemodel)
        {
            if (filemodel == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            else
            {
                var filecontent = filemodel.Image;
                var filetype = filemodel.contentType;
                var filename = filemodel.fileName;

                var bytes = Encoding.ASCII.GetBytes(filecontent);
                var savedFile = HttpContext.Current.Server.MapPath("~/") + filename;

                using (var file = new FileStream(savedFile, FileMode.Create))
                {
                    file.Write(bytes, 0, bytes.Length);
                    file.Flush();
                }

                return Request.CreateResponse(HttpStatusCode.OK, savedFile);
            }
        }
    }

    public class FileModel
    {
        public string contentType { get; set; }

        public string Image { get; set; }

        public string fileName { get; set; }

    }
}
