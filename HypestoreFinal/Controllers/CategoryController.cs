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
    public class CategoryController : Controller
    {
        DepartmentBusiness cb = new DepartmentBusiness();
        Category_Business db = new Category_Business();
        ApplicationDbContext dataAccess = new ApplicationDbContext();

        public ActionResult Index(int? SelectedDepartment)
        {
            ViewBag.SelectedDepartment = new SelectList(cb.all(), "Department_ID", "Department_Name");
            int departmentID = SelectedDepartment.GetValueOrDefault();

            IQueryable<Category> Cat = dataAccess.categories
                            .Where(c => !SelectedDepartment.HasValue || c.Department_ID == departmentID)
                            .OrderBy(d => d.Category_ID)
                            .Include(d => d.Departments);
            return View(Cat.ToList());
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
        [Authorize(Roles ="Admin")]
        public ActionResult Create()
        {
            ViewBag.Department_ID = new SelectList(cb.all(), "Department_ID", "Department_Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category model)
        {
            if (ModelState.IsValid)
            {

                db.add(model);
               // Success(string.Format("<b>{0}</b> was successfully added to the database.", model.Category_Name), true);
                return RedirectToAction("Create");
            }
            ViewBag.Department_ID = new SelectList(cb.all(), "Department_ID", "Department_Name");
            return View(model);
        }
        public ActionResult Edit(int? id)
        {
            ViewBag.Department_ID = new SelectList(cb.all(), "Department_ID", "Department_Name");
            if (id == null)
                return RedirectToAction("Bad_Request", "Error");
            if (db.find_by_id(id) != null)
                return View(db.find_by_id(id));
            else
                return RedirectToAction("Not_Found", "Error");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category model)
        {
            if (ModelState.IsValid)
            {
                db.edit(model);
                // Success(string.Format("<b>{0}</b> was successfully updated.", model.Category_Name), true);
                return RedirectToAction("Index");
            }
            ViewBag.Department_ID = new SelectList(cb.all(), "Department_ID", "Department_Name");
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