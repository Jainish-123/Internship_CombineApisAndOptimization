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
using System.Web.Security;

namespace SampleWebApi_5.Controllers
{
    [RoutePrefix("api/registration")]
    public class registrationController : ApiController
    {
        CombineApiEntities db = new CombineApiEntities();

        [Authorize]

        [Route("FetchAllData")]
        [HttpGet]
        public IHttpActionResult FetchAllData()     //To Optimize make sp
        {
            test obj = new test();
            obj.connection();
            DataSet ds = obj.selectDataRegistration();
            var list = ds.Tables[0].AsEnumerable().Select(dataRow => new registration
            {
                UserId = dataRow.Field<string>("UserId"),
                Password = dataRow.Field<string>("Password"),
                Name = dataRow.Field<string>("Name"),
                Mobile_No = dataRow.Field<string>("Mobile_No"),
                Email = dataRow.Field<string>("Email")
            }).ToList();

            return Ok(list);
        }

        [Route("InsertData")]
        [HttpPost]
        public IHttpActionResult InsertData([FromBody] registration tempObj)
        {
            if (db.registrations.Any(alias => alias.Mobile_No.Equals(tempObj.Mobile_No)))
            {
                return BadRequest("Name already exists.");
            }
            else
            {
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.registrations.Add(tempObj);
                        MembershipUser user = Membership.CreateUser(tempObj.UserId, tempObj.Password);
                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return BadRequest(ex.Message);
                    }
                }
            }
            return Ok();
        }
    }
}
