using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShoppingCartMVC.Models;
using System.IO;
using System.Data;
namespace ShoppingCartMVC.Controllers
{
    public class ProductsController : Controller
    {
        dbOnlineCoffeeShopEntities db = new dbOnlineCoffeeShopEntities();

        #region showing all products for admin 

        public ActionResult Index()
        {
            var query = db.vw_getallproducts.ToList();
            return View(query);
        }

        #endregion


        #region products add for admin

        public ActionResult Create()
        {
            List<tblCategories> list = db.tblCategories.ToList();
            ViewBag.CatList = new SelectList(list, "CatId", "Name");
            return View();
        }



        [HttpPost]
        public ActionResult Create(tblProducts p , HttpPostedFileBase Image)
        {
            List<tblCategories> list = db.tblCategories.ToList();
            ViewBag.CatList = new SelectList(list, "CatId", "Name");
            if (ModelState.IsValid)
            {
                tblProducts pro = new tblProducts();
                pro.Name = p.Name;
                pro.Description = p.Description;
                pro.Unit = p.Unit;
                pro.Image = Image.FileName.ToString();
                pro.CatId = p.CatId;
                //image upload
                var folder = Server.MapPath("~/Uploads/");
                Image.SaveAs(Path.Combine(folder, Image.FileName.ToString()));
                db.tblProducts.Add(pro);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                TempData["msg"] = "Product Not Upload";
            }
            return View();
        }


        #endregion


        #region edit products

        public ActionResult Edit(int id)
        {

            List<tblCategories> list = db.tblCategories.ToList();
            ViewBag.CatList = new SelectList(list, "CatId", "Name");

            var query = db.tblProducts.SingleOrDefault(m => m.ProID == id);

            return View(query);
        }

    
        [HttpPost]
        public ActionResult Edit(tblProducts p, HttpPostedFileBase Image)
        {
            List<tblCategories> list = db.tblCategories.ToList();
            ViewBag.CatList = new SelectList(list, "CatId", "Name");

            try
            {
                p.Image = Image.FileName.ToString();
                var folder = Server.MapPath("~/Uploads/");
                Image.SaveAs(Path.Combine(folder, Image.FileName.ToString()));
                db.Entry(p).State = (System.Data.Entity.EntityState)EntityState.Modified;
                db.SaveChanges();
            }
            catch(Exception ex)
            {
                TempData["msg"] = ex;
            }
            return RedirectToAction("Index");   
        }

        #endregion


        #region delete product 

        public ActionResult Deleted(int id)
        {
            var query = db.tblProducts.SingleOrDefault(m => m.ProID == id); 
            db.tblProducts.Remove(query);
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        #endregion

    }
}