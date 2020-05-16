using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HypestoreFinal.Controllers;
using HypestoreFinal;
using HypestoreFinal.Models;

namespace HypestoreFinal.Controllers
{
    public class OrdersController : Controller
    {
        DepartmentBusiness db = new DepartmentBusiness();

        public ActionResult Index()
        {
            return View(db.all());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
                return RedirectToAction("Bad_Request", "Error");
            if (db.find_by_id(id) != null)
                return View(db.find_by_id(id));
            else
                return RedirectToAction("Not_Found", "Error");
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Department model)
        {
            if (ModelState.IsValid)
            {
                db.add(model);
               //Success(string.Format("<b>{0}</b> was successfully added to the database.", model.Department_Name), true);
                return RedirectToAction("Create");
            }

            return View(model);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("Bad_Request", "Error");
            if (db.find_by_id(id) != null)
                return View(db.find_by_id(id));
            else
                return RedirectToAction("Not_Found", "Error");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Department model)
        {
            if (ModelState.IsValid)
            {
                db.edit(model);
                // Success(string.Format("<b>{0}</b> was successfully updated.", model.Category_Name), true);
                return RedirectToAction("Index");
            }
            return View(model);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return RedirectToAction("Bad_Request", "Error");
            if (db.find_by_id(id) != null)
                return View(db.find_by_id(id));
            else
                return RedirectToAction("Not_Found", "Error");
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.delete(db.find_by_id(id));
            if (db.find_by_id(id) != null)
            {
                //  Danger(string.Format("<b>{0}</b> was successfully deleted.", db.find_by_id(id).Department_Name), true);
            }
            return RedirectToAction("Index");
        }
    }
}