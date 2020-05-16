using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HypestoreFinal.Models;

namespace HypestoreFinal.Controllers
{
    public class Items1Controller : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Items1
        public ActionResult Index()
        {
            var items = db.Items.Include(i => i.Categories).Include(i => i.suppliers);
            return View(items.ToList());
        }

        // GET: Items1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // GET: Items1/Create
        public ActionResult Create()
        {
            ViewBag.Category_ID = new SelectList(db.categories, "Category_ID", "Category_Name");
            ViewBag.SupplierId = new SelectList(db.Suppliers, "SupplierID", "Name");
            return View();
        }

        // POST: Items1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ItemCode,ItemId,Category_ID,Brand,SupplierId,Name,Description,size,color,SkuNo,Price,ReOrder_Level,QuantityInStock,ReOrder_Quantity,ReOrder_Percent,Image,isDiscontinued")] Item item)
        {
            if (ModelState.IsValid)
            {
                db.Items.Add(item);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Category_ID = new SelectList(db.categories, "Category_ID", "Category_Name", item.Category_ID);
            ViewBag.SupplierId = new SelectList(db.Suppliers, "SupplierID", "Name", item.SupplierId);
            return View(item);
        }

        // GET: Items1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            ViewBag.Category_ID = new SelectList(db.categories, "Category_ID", "Category_Name", item.Category_ID);
            ViewBag.SupplierId = new SelectList(db.Suppliers, "SupplierID", "Name", item.SupplierId);
            return View(item);
        }

        // POST: Items1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ItemCode,ItemId,Category_ID,Brand,SupplierId,Name,Description,size,color,SkuNo,Price,ReOrder_Level,QuantityInStock,ReOrder_Quantity,ReOrder_Percent,Image,isDiscontinued")] Item item)
        {
            if (ModelState.IsValid)
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Category_ID = new SelectList(db.categories, "Category_ID", "Category_Name", item.Category_ID);
            ViewBag.SupplierId = new SelectList(db.Suppliers, "SupplierID", "Name", item.SupplierId);
            return View(item);
        }

        // GET: Items1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Item item = db.Items.Find(id);
            db.Items.Remove(item);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
