using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShoppingCartMVC.Models;

namespace ShoppingCartMVC.Controllers
{
    public class TableController : Controller
    {
        dbOnlineCoffeeShopEntities db = new dbOnlineCoffeeShopEntities();
        
        #region Showing tables for admin
        public ActionResult Index()
        {
            var query = db.tblTable.ToList();
            return View(query);
        }
        #endregion

        #region add categories
        public ActionResult Create()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Inside", Value = "inside" });
            items.Add(new SelectListItem { Text = "Outside", Value = "ouside" });
            ViewBag.AreaList = items;
            return View();
        }

        [HttpPost]
        public ActionResult Create(tblTable t)
        {
            if (ModelState.IsValid)
            {
                tblTable tbl = new tblTable();
                //tbl.area = table.area;
                tbl.area = t.area;
                tbl.numSits = 0;
                db.tblTable.Add(tbl);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                TempData["msg"] = "Didn't create table";
            }
            return View();
        }
        #endregion

        #region Edit table
        public ActionResult Edit(int id)
        {
            var query = db.tblTable.SingleOrDefault(m => m.tableId == id);
            return View(query);
        }

        [HttpPost]
        public ActionResult Edit(tblTable tbl,string areas)
        {
            try
            {
                var query = db.tblTable.SingleOrDefault(m => m.tableId == tbl.tableId);
                //if (tbl.numSeats != null)
                //    query.numSeats = tbl.numSeats;
                query.area = tbl.area;
                db.Entry(query).State = (System.Data.Entity.EntityState)EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex;
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region delete category
        public ActionResult Delete(int id)
        {
            //int temp = (int)TempData["tableId"];
            var query = db.tblTable.SingleOrDefault(m => m.tableId == id);
            db.tblTable.Remove(query);
            db.SaveChanges();
            //return View("Index");
            return RedirectToAction("Index");
        }
        #endregion
    }
}