using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HypestoreFinal.Helpers;
using HypestoreFinal.Models;
using HypestoreFinal.ViewModels;



namespace HypestoreFinal.Controllers
{
    public class ItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private Item_Business ib = new Item_Business();
        Category_Business cb = new Category_Business();
        public string shoppingCartID { get; set; }
        public const string CartSessionKey = "CartId";
        public ItemsController() { }

        public ActionResult Index()
        {
            return View(ib.all());
        }

        public ActionResult Index_OTS()
        {
            return View(ib.all());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return RedirectToAction("Bad_Request", "Error");
            if (ib.find_by_id(id) != null)
                return View(ib.find_by_id(id));
            else
                return RedirectToAction("Not_Found", "Error");
        }

        public ActionResult Manage_Index()
        {

            return View();
        }

        public ActionResult Creates()
        {
            ViewBag.Category_ID = new SelectList(cb.all(), "Category_ID", "Category_Name");
            ViewBag.SupplierId = new SelectList(db.Suppliers, "SupplierID", "Name");
            return View();
        }
        public ActionResult Create()
        {
            ViewBag.Category_ID = new SelectList(cb.all(), "Category_ID", "Category_Name");
            ViewBag.SupplierId = new SelectList(db.Suppliers, "SupplierID", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Item model, HttpPostedFileBase img_upload)
        {
            ViewBag.Category_ID = new SelectList(cb.all(), "Category_ID", "Category_Name");
            ViewBag.SupplierId = new SelectList(db.Suppliers, "SupplierID", "Name");
            byte[] data = null;
            data = new byte[img_upload.ContentLength];
            img_upload.InputStream.Read(data, 0, img_upload.ContentLength);
            model.Image = data;
            //model.Quantity_In_Stock = ib.Calc_Quantity();
            //  model.ReOrder_Level = ib.Calc_ReOrder();
            if (ModelState.IsValid)
            {
                ib.add(model);
                // Success(string.Format("<b>{0}</b> was successfully added to the database.", model.Name), true);

                return View(model);
            }
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int? id)
        {
            ViewBag.Category_ID = new SelectList(cb.all(), "Category_ID", "Category_Name");
            if (id == null)
                return RedirectToAction("Bad_Request", "Error");
            if (ib.find_by_id(id) != null)
                return View(ib.find_by_id(id));
            else
                return RedirectToAction("Not_Found", "Error");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Item model, HttpPostedFileBase img_upload)
        {
            byte[] data = null;
            data = new byte[img_upload.ContentLength];
            img_upload.InputStream.Read(data, 0, img_upload.ContentLength);
            model.Image = data;
            if (ModelState.IsValid)
            {
                ib.edit(model);
                return RedirectToAction("Index");
            }
            ViewBag.Category_ID = new SelectList(cb.all(), "Category_ID", "Category_Name");
            return View(model);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return RedirectToAction("Bad_Request", "Error");
            if (ib.find_by_id(id) != null)
                return View(ib.find_by_id(id));
            else
                return RedirectToAction("Not_Found", "Error");
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ib.delete(ib.find_by_id(id));
            return RedirectToAction("Index");
        }
        [ChildActionOnly]
        public ActionResult BelowReOrderLevel()
        {
            return PartialView("_ReorderLevels", ib.Below_Order_Level());
        }

        //
        [ChildActionOnly]
        public ActionResult OutOfStock()
        {

            return PartialView("_OutOfStock", ib.Out_Of_Stock());
        }

        public ActionResult StockLevels()
        {
            return View();
        }

        public ActionResult ManageItems(int? id)
        {
            ViewBag.Category_ID = new SelectList(cb.all(), "Category_ID", "Category_Name");
            if (ib.find_by_id(id) != null)
                return View(ib.find_by_id(id));
            else
                return RedirectToAction("Not_Found", "Error");
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult ManageItems(Item model, int? value)
        //{
        //    ViewBag.Category_ID = new SelectList(cb.all(), "Category_ID", "Category_Name");

        //    if (ModelState.IsValid)
        //    {
        //        ib.update(model);
        //        //Success(string.Format("<b>{0}</b> was successfully updated.", model.Name), true);
        //        return View();
        //    }
        //    //Danger(string.Format("<b>{0}</b> could not update.", model.Name), true);
        //    ViewBag.Category_ID = new SelectList(cb.all(), "Category_ID", "Category_Name");
        //    return View(model);
        //}
        public ActionResult Purchase(int id)
        {
            var item = ib.findItem_by_id(id);
            Supplier s = new Supplier();
            PurchaseOrder p = new PurchaseOrder();
            List<Item> l = new List<Item>();
            if(item!=null)
            {
                ib.Add_Item_To_PurchaseList(s, item.ItemCode);
                ib.Add_PurchaseOrder_Item(p);
            }

            return RedirectToAction("StockLevels");
        }
        public ActionResult PurchaseList(int? id)
        {
            var i = db.purchaseOrders.Find(id);
            ViewBag.Items = db.purchaseOrderItems.ToList();
            return View();
        }
        //    public ActionResult Fall_catalog()
        //{
        //    return View(ib.all());
        //}


        public string GetCartID()
        {
            if (System.Web.HttpContext.Current.Session[CartSessionKey] == null)
            {
                if (!String.IsNullOrWhiteSpace(System.Web.HttpContext.Current.User.Identity.Name))
                {
                    System.Web.HttpContext.Current.Session[CartSessionKey] = System.Web.HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    Guid temp_cart_ID = Guid.NewGuid();
                    System.Web.HttpContext.Current.Session[CartSessionKey] = temp_cart_ID.ToString();
                }
            }
            return System.Web.HttpContext.Current.Session[CartSessionKey].ToString();
        }
    }
}