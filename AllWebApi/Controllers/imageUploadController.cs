using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.UI.WebControls;
using System.Net.Http.Headers;
using AllWebApi.Models;
using classADO_AllWebApi;
using Image = AllWebApi.Models.Image;
using System.Data.Entity;

namespace SampleWebApi_6.Controllers
{
    public class imageUploadController : ApiController
    {
        CombineApiEntities db = new CombineApiEntities();
        List<string> validExtensions = new List<string>() {".jpg", ".jpeg", ".png" };
        string upFileName, ext, filepath;

        [Route("api/imagetUpload/uploadImage")]
        [HttpPost]
        public string uploadImage()            
        {
            if(!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            using(DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    string root = HttpContext.Current.Server.MapPath("~/App_Data");

                    for (int i = 0; i < HttpContext.Current.Request.Files.Count; i++)
                    {
                        HttpPostedFile file = HttpContext.Current.Request.Files[i];

                        upFileName = Path.GetFileName(file.FileName).Trim('"');

                        ext = Path.GetExtension(upFileName);

                        System.Drawing.Image myImage = System.Drawing.Image.FromStream(file.InputStream);

                        if ((file.ContentLength < 900000)
                            && (validExtensions.Contains(ext, StringComparer.OrdinalIgnoreCase))
                            && (myImage.Height <= 1000 && myImage.Width <= 1000))
                        {
                            continue;
                        }
                        else
                        {
                            return $"{upFileName} : Image size is too large or Image type is not supported or Image dimensions too large...!.";
                        }
                    }

                    for (int i = 0; i < HttpContext.Current.Request.Files.Count; i++)
                    {
                        HttpPostedFile file = HttpContext.Current.Request.Files[i];

                        upFileName = Path.GetFileName(file.FileName).Trim('"');

                        ext = Path.GetExtension(upFileName);

                        filepath = Path.Combine(root, upFileName);

                        file.SaveAs(filepath);

                        db.Images.Add(new Image { Image1 = filepath });
                    }
                    db.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return ex.Message;
                }
            }
            return "Image uploaded.";
        }
    }
}
