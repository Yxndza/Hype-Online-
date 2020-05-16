using HypestoreFinal.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypestoreFinal
{
   public class DepartmentBusiness
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public List<Department> all()
        {
            return db.Departments.ToList();
        }
        public bool add(Department model)
        {
            try
            {
                db.Departments.Add(model);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            { return false; }
        }
        public bool edit(Department model)
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
        public bool delete(Department model)
        {
            try
            {
                db.Departments.Remove(model);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            { return false; }
        }
        public Department find_by_id(int? id)
        {
            return db.Departments.Find(id);
        }
        public List<Category> Department_Category(int? id)
        {
            return find_by_id(id).categories.ToList();
        }
       
    }
}
