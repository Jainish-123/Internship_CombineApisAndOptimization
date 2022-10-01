using AllWebApi.Models;
using classADO_AllWebApi;
using SampleWebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SampleWebApi_3.Controllers
{
    [RoutePrefix("api/Patient")]
    public class PatientController : ApiController
    {
        CombineApiEntities db = new CombineApiEntities();

        [Route("FetchAllData")]
        [HttpGet]
        public IHttpActionResult FetchAllData()
        {
            test obj = new test();
            obj.connection();
            DataSet ds = obj.spSelectData("spGetPatient");
            var list = ds.Tables[0].AsEnumerable().Select(dataRow => new Patient
            {
                Pat_Id = dataRow.Field<string>("Pat_Id"),
                Pat_Name = dataRow.Field<string>("Pat_Name"),
                Pat_Age = dataRow.Field<int>("Pat_Age")
            }).ToList();

            return Ok(list);
        }

        [Route("FetchSpecificData")]
        [HttpGet]
        public IHttpActionResult FetchSpecificData(string id)
        {
            test obj = new test();
            obj.connection();
            DataSet ds = obj.spSelectData($"spGetPatientById {id}");
            var list = ds.Tables[0].AsEnumerable().Select(dataRow => new Patient
            {
                Pat_Id = dataRow.Field<string>("Pat_Id"),
                Pat_Name = dataRow.Field<string>("Pat_Name"),
                Pat_Age = dataRow.Field<int>("Pat_Age")
            }).ToList();

            return Ok(list);
        }

        [Route("insertData")]
        [HttpPost]
        public IHttpActionResult insertData([FromBody] Patient tempObj)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    Patient pat = new Patient();
                    pat.Pat_Id = tempObj.Pat_Id;
                    pat.Pat_Name = tempObj.Pat_Name;
                    pat.Pat_Age = tempObj.Pat_Age;

                    if (tempObj.Doc_Name != null)
                    {
                        DocPat docPat = new DocPat();
                        docPat.Doc_Name = tempObj.Doc_Name;
                        docPat.Pat_Name = tempObj.Pat_Name;

                        db.DocPats.Add(docPat);
                    }

                    db.Patients.Add(pat);
                    db.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest(ex.Message);
                }
            }

            return Ok();
        }

        [Route("insertArrData")]
        [HttpPost]
        public IHttpActionResult insertArrData([FromBody] Patient[] tempObj)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    for (int i = 0; i < tempObj.Length; i++)
                    {
                        Patient pat = new Patient();
                        pat.Pat_Id = tempObj[i].Pat_Id;
                        pat.Pat_Name = tempObj[i].Pat_Name;
                        pat.Pat_Age = tempObj[i].Pat_Age;

                        if (tempObj[i].Doc_Name != null)
                        {
                            DocPat docPat = new DocPat();
                            docPat.Doc_Name = tempObj[i].Doc_Name;
                            docPat.Pat_Name = tempObj[i].Pat_Name;

                            db.DocPats.Add(docPat);
                        }

                        db.Patients.Add(pat);
                    }
                    db.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest(ex.Message);
                }
            }

            return Ok();
        }
    }
}
