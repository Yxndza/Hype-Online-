
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HypestoreFinal.Models;
using HypestoreFinal.ViewModels;

namespace HypestoreFinal
{
    public class Customer_Business
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        public List<Customer> all()
        {
            return db.Customers.ToList();
        }

        public IEnumerable<SelectListItem> GetOwners()
        {
            using (var context = new ApplicationDbContext())
            {
                List<SelectListItem> owner = context.Customers.AsNoTracking()
                    .OrderBy(n => n.FirstName)
                        .Select(n =>
                        new SelectListItem
                        {
                            Value = n.CustomerId.ToString(),
                            Text = n.FirstName
                        }).ToList();
                var countrytip = new SelectListItem()
                {
                    Value = null,
                    Text = "--- select owner ---"
                };
                owner.Insert(0, countrytip);
                return new SelectList(owner, "Value", "Text");
            }
        }

        public bool add(Customer model)
        {
            try
            {
                db.Customers.Add(model);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            { return false; }
        }
        public bool edit(Customer model)
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
        public Customer find_by_id(int? id)
        {
            return db.Customers.Find(id);
        }

      
    }
}