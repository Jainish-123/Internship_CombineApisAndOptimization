using classADO_AllWebApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Http;

namespace SampleWebApi_11.Controllers
{
    public class employeeController : ApiController
    {
        List<string> ValidExtensions = new List<string>() { ".xlsx"};
        string upFileName, ext, filepath;

        [Route("api/employee/insertData")]
        [HttpPost]
        public string insertData()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            try
            {
                HttpPostedFile file = HttpContext.Current.Request.Files[0];

                string root = HttpContext.Current.Server.MapPath("~/App_Data");

                upFileName = Path.GetFileName(file.FileName).Trim('"');

                ext = Path.GetExtension(upFileName);

                if (ValidExtensions.Contains(ext, StringComparer.OrdinalIgnoreCase) == false)
                {
                    return $"{upFileName} : File type is not supported...!";
                }

                filepath = Path.Combine(root, upFileName);

                test obj = new test();

                file.SaveAs(filepath);

                obj.insertDataIntoDatabase(filepath);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "Data Inserted";
        }
    }
}
