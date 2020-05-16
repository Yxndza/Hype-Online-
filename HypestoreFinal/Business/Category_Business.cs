using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using HypestoreFinal.Models;
using HypestoreFinal.ViewModels;

namespace HypestoreFinal
{
    public class Category_Business
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public List<Category> all()
        {

            return db.categories.Include(i => i.Departments).ToList();
        }
        public List<Category> all2(int? id, int? ItemID)
        {
            var viewModel = new DepartmentIndexData();

            viewModel.Categories = db.categories
                    .Include(i => i.Departments)
                    .Include(i => i.Items.Select(c => c.Name))
                    .OrderBy(i => i.Category_ID);

            //if (id != null)
            //{
            //    ViewBag.ItemID = id.Value;
            //    viewModel.Items = viewModel.Categories.Where(
            //        i => i.Category_ID == id.Value).Single().Items;
            //}


            return db.categories.Include(i => i.Departments).ToList();
        }
        public bool add(Category model)
        {
            try
            {
                db.categories.Add(model);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            { return false; }
        }
        public bool edit(Category model)
        {
            try
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            { return false; }
        }
        public bool delete(Category model)
        {
            try
            {
                db.categories.Remove(model);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            { return false; }
        }
        public Category find_by_id(int? id)
        {
            return db.categories.Find(id);
        }
        public List<Item> category_items(int? id)
        {
            return find_by_id(id).Items.ToList();
        }
    }
}