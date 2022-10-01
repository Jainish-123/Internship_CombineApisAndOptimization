using AllWebApi.Models;
using classADO_AllWebApi;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace SampleWebApi.Controllers
{
    [RoutePrefix("api/StudentApi")]
    public class StudentApiController : ApiController
    {
        CombineApiEntities db = new CombineApiEntities();

        [Route("FetchAllData")]
        [HttpGet]
        public IHttpActionResult FetchAllData()     //To Optimize make sp
        {
            test obj = new test();
            obj.connection();
            DataSet ds = obj.selectData("Practice_Student");
            return Ok(ds);
        }

        [Route("FetchSpecificData")]
        [HttpGet]
        public IHttpActionResult FetchSpecificData(string id)       //To Optimize make sp
        {
            var obj = db.Practice_Student.Where(model => model.id == id).FirstOrDefault();
            return Ok(obj);
        }

        [Route("InsertData")]
        [HttpPost]
        public IHttpActionResult InsertData([FromBody] Practice_Student tempObj)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    db.Practice_Student.Add(tempObj);
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

        [Route("UpdateData")]
        [HttpPut]
        public IHttpActionResult UpdateData([FromBody] Practice_Student tempObj)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    db.Entry(tempObj).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BadRequest(ex.Message);
                }
            }
            //var obj = db.Practice_Student.Where(model => model.id == tempObj.id).FirstOrDefault();
            //if(obj != null)
            //{
            //    obj.name = tempObj.name;
            //    obj.sem = tempObj.sem;
            //    obj.cpi = tempObj.cpi;
            //    db.SaveChanges();
            //}
            //else
            //{
            //    return NotFound();
            //}

            return Ok();
        }

        [Route("DeleteData")]
        [HttpDelete]
        public IHttpActionResult DeleteData(string id)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var obj = db.Practice_Student.Where(model => model.id == id).FirstOrDefault();
                    db.Entry(obj).State = System.Data.Entity.EntityState.Deleted;
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
