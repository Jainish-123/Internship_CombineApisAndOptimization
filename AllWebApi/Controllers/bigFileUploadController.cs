using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace SampleWebApi_9.Controllers
{
    public class bigFileUploadController : ApiController
    {
        List<string> notValidExtensions = new List<string>() { ".exe", ".bat" };
        string upFileName, ext, filepath;


        [Route("api/bigFileUpload/uploadBigFile")]
        [HttpPost]
        public string uploadBigFile()   
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            try
            {
                string root = HttpContext.Current.Server.MapPath("~/App_Data");

                for (int i = 0; i < HttpContext.Current.Request.Files.Count; i++)
                {
                    HttpPostedFile file = HttpContext.Current.Request.Files[i];

                    upFileName = Path.GetFileName(file.FileName).Trim('"');

                    ext = Path.GetExtension(upFileName);

                    if (notValidExtensions.Contains(ext, StringComparer.OrdinalIgnoreCase))
                    {
                        return $"{upFileName} : File type is not supported...!";
                    }
                    else
                    {
                        continue;
                    }
                }

                for (int i = 0; i < HttpContext.Current.Request.Files.Count; i++)
                {
                    HttpPostedFile file = HttpContext.Current.Request.Files[i];

                    upFileName = Path.GetFileName(file.FileName).Trim('"');

                    ext = Path.GetExtension(upFileName);

                    filepath = Path.Combine(root, upFileName);

                    file.SaveAs(filepath);
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "File uploaded.";
        }
    }
}
