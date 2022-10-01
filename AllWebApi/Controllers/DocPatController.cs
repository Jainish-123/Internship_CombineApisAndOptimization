using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using classADO_AllWebApi;
using AllWebApi.Models;

namespace SampleWebApi_3.Controllers
{
    [RoutePrefix("api/DocPat")]
    public class DocPatController : ApiController
    {
        CombineApiEntities db = new CombineApiEntities();

        [Route("FetchAllData")]
        [HttpGet]
        public IHttpActionResult FetchAllData()
        {
            test obj = new test();
            obj.connection();
            DataSet ds = obj.spSelectData("spGetDocPatient");
            var list = ds.Tables[0].AsEnumerable().Select(dataRow => new DocPat
            {
                Doc_Name = dataRow.Field<string>("Doc_Name"),
                Pat_Name = dataRow.Field<string>("Pat_Name")
            }).ToList();

            return Ok(list);
        }

        [Route("FetchSpecificData")]
        [HttpGet]
        public IHttpActionResult FetchSpecificData(string name)
        {
            test obj = new test();
            obj.connection();
            DataSet ds = obj.spSelectData($"spGetDocPatByDocName {name}");
            var list = ds.Tables[0].AsEnumerable().Select(dataRow => new DocPat
            {
                Doc_Name = dataRow.Field<string>("Doc_Name"),
                Pat_Name = dataRow.Field<string>("Pat_Name")
            }).ToList();

            return Ok(list);
        }
    }
}
