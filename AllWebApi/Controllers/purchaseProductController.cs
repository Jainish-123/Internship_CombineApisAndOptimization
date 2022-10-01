using AllWebApi.Models;
using classADO_AllWebApi;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http;
using System.Windows.Documents;

namespace SampleWebApi_12.Controllers
{
    [RoutePrefix("api/purchaseProduct")]
    public class purchaseProductController : ApiController
    {
        CombineApiEntities db = new CombineApiEntities();

        purchase purObj = new purchase();

        double total_amount = 0;

        [Route("selectData")]
        [HttpGet]
        public IHttpActionResult selectData()
        {
            test obj = new test();
            obj.connection();

            DataSet ds1 = obj.spSelectData("spGetPurchase");
            var list1 = ds1.Tables[0].AsEnumerable().Select(dataRow => new purchase
            {
                pur_id = dataRow.Field<int>("pur_id"),
                pur_no = dataRow.Field<string>("pur_no"),
                date = dataRow.Field<DateTime>("date"),
                total_amount = dataRow.Field<double>("total_amount")
            }).ToList();
            
            int tmpPurId;
            //List<List<purchase_product>> finalList = new List<List<purchase_product>>();

            foreach (var item in list1)
            {
                tmpPurId = item.pur_id;
                DataSet ds2 = obj.spSelectData($"spGetProducts {tmpPurId}");
                var list2 = ds2.Tables[0].AsEnumerable().Select(dataRow => new purchase_product
                {
                    pur_pro_id = dataRow.Field<int>("pur_pro_id"),
                    pur_id = dataRow.Field<int>("pur_id"),
                    item = dataRow.Field<string>("item"),
                    qty = dataRow.Field<int>("qty"),
                    amount = dataRow.Field<double>("amount")
                }).ToList();

                item.purchaseProductForList = list2;

                //finalList.Add(list2);
            }
            return Ok(list1);
        }

        [Route("selectDataOptimized")]
        [HttpGet]
        public IHttpActionResult selectDataOptimized()
        {
            test obj = new test();
            obj.connection();
            DataSet ds = obj.spSelectData("spGetBothPurPro");

            var list1 = ds.Tables[0].AsEnumerable().Select(dataRow => new purchase_product
            {
                pur_pro_id = dataRow.Field<int>("pur_pro_id"),
                pur_id = dataRow.Field<int>("pur_id"),
                item = dataRow.Field<string>("item"),
                qty = dataRow.Field<int>("qty"),
                amount = dataRow.Field<double>("amount")
            }).ToList();

            var list2 = ds.Tables[1].AsEnumerable().Select(dataRow => new purchase
            {
                pur_id = dataRow.Field<int>("pur_id"),
                pur_no = dataRow.Field<string>("pur_no"),
                date = dataRow.Field<DateTime>("date"),
                total_amount = dataRow.Field<double>("total_amount"),
                purchaseProductForList = list1.Where(alias => alias.pur_id == dataRow.Field<int>("pur_id")).ToList()
            }).ToList();

            return Ok(list2);
        }

        [Route("selectDataOptimizedByJoin")]
        [HttpGet]
        public IHttpActionResult selectDataOptimizedByJoin()
        {
            test obj = new test();
            obj.connection();
            DataSet ds = obj.spSelectData("spGetBothPurProInnerJoin");

            var list = ds.Tables[0].AsEnumerable().Select(dataRow => new purchase
            {
                pur_id = dataRow.Field<int>("pur_id"),
                pur_no = dataRow.Field<string>("pur_no"),
                date = dataRow.Field<DateTime>("date"),
                total_amount = dataRow.Field<double>("total_amount"),
                purchaseProductForList = ds.Tables[0].AsEnumerable().Select(dataRow2 => new purchase_product
                    {
                        pur_pro_id = dataRow2.Field<int>("pur_pro_id"),
                        pur_id = dataRow2.Field<int>("pur_id"),
                        item = dataRow2.Field<string>("item"),
                        qty = dataRow2.Field<int>("qty"),
                        amount = dataRow2.Field<double>("amount")
                    }).Where(a => a.pur_id == dataRow.Field<int>("pur_id")).ToList()

            }).DistinctBy(a => a.pur_id).ToList();

             return Ok(list);            
        }

        [Route("insertData")]
        [HttpPost]
        public IHttpActionResult insertData([FromBody] purchase_product[] req)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    test obj = new test();

                    for (int i = 0; i < req.Length; i++)
                    {
                        total_amount += req[i].amount;
                    }

                    //total_amount = db.purchase_product.Where(alias => alias.pur_id == 1).Sum(p => p.amount);

                    purObj.pur_no = obj.getUniqueNumber();
                    purObj.date = DateTime.Now;
                    purObj.total_amount = total_amount;

                    db.purchases.Add(purObj);

                    db.SaveChanges();

                    int purId = purObj.pur_id;

                    for (int i = 0; i < req.Length; i++)
                    {
                        req[i].pur_id = purId;
                        db.purchase_product.Add(req[i]);
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


        [Route("deleteDataInBoth")]
        [HttpDelete]
        public IHttpActionResult deleteDataInBoth([FromBody] int id)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var obj1 = db.purchases.Where(alias => alias.pur_id == id).FirstOrDefault();
                    db.Entry(obj1).State = EntityState.Deleted;
                    db.purchase_product.RemoveRange(db.purchase_product.Where(alias => alias.pur_id == id));
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

        
        [Route("deleteDataInPurchase_Product")]
        [HttpDelete]
        public IHttpActionResult deleteDataInPurchase_Product([FromBody] int id)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var obj1 = db.purchase_product.Where(alias => alias.pur_pro_id == id).FirstOrDefault();
                    double tmpAmount = obj1.amount;
                    db.Entry(obj1).State = EntityState.Deleted;
                    var obj2 = db.purchases.Where(alias => alias.pur_id == obj1.pur_id).FirstOrDefault();
                    double tmpTotalAmount = obj2.total_amount;
                    tmpTotalAmount = tmpTotalAmount - tmpAmount;
                    obj2.total_amount = tmpTotalAmount;
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

        [Route("updateData")]
        [HttpPut]
        public IHttpActionResult updateData(purchase_product req)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var obj1 = db.purchase_product.Where(alias => alias.pur_pro_id == req.pur_pro_id).FirstOrDefault();
                    obj1.qty = req.qty;
                    double tmpAmount = obj1.amount - req.amount;
                    obj1.amount = req.amount;
                    var obj2 = db.purchases.Where(alias => alias.pur_id == obj1.pur_id).FirstOrDefault();
                    double tmpTotalAmount = obj2.total_amount;
                    tmpTotalAmount = tmpTotalAmount - tmpAmount;
                    obj2.total_amount = tmpTotalAmount;
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
