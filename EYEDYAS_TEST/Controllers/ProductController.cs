using EYEDYAS_TEST.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EYEDYAS_TEST.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        product_dbEntities db = new product_dbEntities();


        public ActionResult Index()
        {
            ViewBag.ReturnMsg = TempData["status"] == null ? string.Empty : Convert.ToString(TempData["status"]);
            return View(db.PRODUCT_TB.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(HttpPostedFileBase file,PRODUCT_TB prod)
        {
            string filename = Path.GetFileName(file.FileName);
            string _filename = DateTime.Now.ToString("yymmssfff") + filename;
            string extension = Path.GetExtension(file.FileName);
            string path = Path.Combine(Server.MapPath("~/images/"), _filename);
            prod.PIMAGE = "~/images/" + _filename;

            if (extension.ToLower()==".jpg" || extension.ToLower() == "jpeg" || extension.ToLower() == "png")
            {
                if(file.ContentLength < 9000000)
                {
                    db.PRODUCT_TB.Add(prod);
                    if(db.SaveChanges() > 0)
                    {
                        file.SaveAs(path);
                        TempData["status"] = "Product Inserted Successfully";
                        
                    }


                }
            }

            return RedirectToAction("Index");
        }


        [HttpGet]       
        public ActionResult Edit(int id)
        {
            var prod = db.PRODUCT_TB.Where(x => x.PID == id).FirstOrDefault();

            return View(prod);
        }

        [HttpPost]
        public ActionResult Edit(HttpPostedFileBase file, PRODUCT_TB prod)
        {
            string filename = Path.GetFileName(file.FileName);
            string _filename = DateTime.Now.ToString("yymmssfff") + filename;
            string extension = Path.GetExtension(file.FileName);
            string path = Path.Combine(Server.MapPath("~/images/"), _filename);
            prod.PIMAGE = "~/images/" + _filename;

            if (extension.ToLower() == ".jpg" || extension.ToLower() == "jpeg" || extension.ToLower() == "png")
            {
                if (file.ContentLength < 1000000)
                {
                    PRODUCT_TB prodtmp = db.PRODUCT_TB.Where(x => x.PID == prod.PID).FirstOrDefault();
                    prodtmp.PIMAGE = prod.PIMAGE;
                    prodtmp.PRICE = prod.PRICE;
                    prodtmp.PTITLE = prod.PTITLE;
                    prodtmp.STOCK = prod.STOCK;                  
                    db.Entry(prodtmp).State = System.Data.Entity.EntityState.Modified;
                    if (db.SaveChanges() > 0)
                    {
                        file.SaveAs(path);
                        TempData["status"] = "Product Updated Successfully";
                        
                    }


                }
            }

            return RedirectToAction("Index");
        }

       
        public ActionResult Delete(int id)
        {
            var prod = db.PRODUCT_TB.Where(x => x.PID == id).FirstOrDefault();
            db.Entry(prod).State = System.Data.Entity.EntityState.Deleted;
            if(db.SaveChanges()>0)
            {
                TempData["status"] = "Product Deleted Successfully";
            }

            return RedirectToAction("Index");
        }

        public ActionResult Chart()
        {            

            return View();
        }

        [HttpPost]
        public ActionResult Chart(int range1, int range2)// called with jquery for specific range
        {
            var prod = db.PRODUCT_TB.Where(d => (double)d.PRICE >= (double)range1
                                          && (double)d.PRICE <= (double)range2).ToList();
            return Json(prod, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetChartAll() 
        {
            var prod = db.PRODUCT_TB.ToList();
            return Json(prod, JsonRequestBehavior.AllowGet);
        }
    }
}