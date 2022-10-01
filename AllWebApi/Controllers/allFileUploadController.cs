using AllWebApi.Models;
using classADO_AllWebApi;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace SampleWebApi_7.Controllers
{
    public class allFileUploadController : ApiController
    {
        CombineApiEntities db = new CombineApiEntities();
        string upFileName, ext, filepath;

        [Route("api/allFileUpload/uploadAllFile")]
        [HttpPost]
        public string uploadAllFile()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    string root = HttpContext.Current.Server.MapPath("~/App_Data");
                    for (int i = 0; i < HttpContext.Current.Request.Files.Count; i++)
                    {
                        HttpPostedFile file = HttpContext.Current.Request.Files[i];

                        upFileName = Path.GetFileName(file.FileName).Trim('"');

                        filepath = Path.Combine(root, upFileName);

                        file.SaveAs(filepath);

                        db.AllFiles.Add(new AllFile { File_Path=filepath});
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

            return "File uploaded.";
        }
    }
}
