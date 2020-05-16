using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HypestoreFinal.Models;
using HypestoreFinal.ViewModels;

namespace HypestoreFinal
{
    public class Order_Business
    {
        private ApplicationDbContext db = new ApplicationDbContext();
     
        public List<Order> cust_all()
        {
            return db.Orders.ToList();
        }
        public List<Order> cust_find_by_status(string status)
        {
            return db.Orders.Where(p => p.status.ToLower() == status.ToLower()).ToList();
        }
        public Order cust_find_by_id(int? id)
        {
            return db.Orders.Find(id);
        }
        public List<OrderItem> cust_Order_items(string id)
        {
            return cust_find_by_id(Convert.ToInt32(id)).Order_Items.ToList();
        }

        public object cust_find_by_id(string id)
        {
            throw new NotImplementedException();
        }

       
    }
       
    }
