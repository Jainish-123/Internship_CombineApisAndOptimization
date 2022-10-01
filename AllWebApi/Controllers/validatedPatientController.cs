using AllWebApi.Models;
using classADO_AllWebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AllWebApi.Controllers
{
    public class validatedPatientController : ApiController
    {
        CombineApiEntities db = new CombineApiEntities();

        [Route("api/validatedPatient/insertData")]
        [HttpPost]
        public IHttpActionResult insertData([FromBody] Patient tempObj)
        {

            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            Patient pat = new Patient();
            pat.Pat_Id = tempObj.Pat_Id;
            pat.Pat_Name = tempObj.Pat_Name;
            pat.Pat_Age = tempObj.Pat_Age;

            db.Patients.Add(pat);

            db.SaveChanges();

            return Ok();
        }
    }
}
