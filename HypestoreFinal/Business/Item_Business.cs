using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using HypestoreFinal.Controllers;
using HypestoreFinal.Models;
using HypestoreFinal.ViewModels;

namespace HypestoreFinal
{
    public class Item_Business
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private Item item = new Item();
        private Category category = new Category();
        private SuppliersController supplier = new SuppliersController();
        public PurchaseOrder purchaseOrder = new PurchaseOrder();
        public  PurchaseOrderItem PurchaseOrderItem = new PurchaseOrderItem();

        public List<Item> all()
        {
            var items = db.Items.Include(i => i.Categories).Include(i => i.suppliers);

            return(items.ToList());
          

        }
        public bool add(Item model)
        {
            try
            {
                db.Items.Add(model);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            { return false; }
        }
        public bool edit(Item model)
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
        public bool delete(Item model)
        {
            try
            {
                db.Items.Remove(model);
                db.SaveChanges();
                return true;
            }
            catch (Exception )
            { return false; }
        }
        public Item find_by_id(int? id)
        {
            return db.Items.Find(id);
        }
        public List<Item> find_by_Department(string department)
        {
            var dep = db.Departments.ToList().Where(x => x.Department_Name == department).FirstOrDefault();

            return db.Items.ToList().Where(x => x.Categories.Departments.Department_Name == dep.ToString()).ToList();
        }

        //methods for stock management
        //public bool update(Item model)
        //{
        //    try
        //    {
        //        var update = db.Items.Find(model.ItemCode);
        //        var cat = db.categories.Where(x => x.Category_ID == model.Category_ID).Select(x => x.Safety_Stock_level).FirstOrDefault();

        //        update.QuantityInStock = update.QuantityInStock + model.QTY_Received;
        //        update.Price = ((update.Price * update.Mark_Up) / 100) + (update.Price);
        //        update.ReOrder_Level = (cat - (cat * model.ReOrder_Percent) / 100);


        //        db.Entry(update).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    { return false; }
        //}


        public List<Out_of_stock_VM> Out_Of_Stock()
        {
            var itms = db.Items.ToList();
            //var PE = db.Items.ToList().Where(x => ).Select(p => p.platformID).Single();

            List<Out_of_stock_VM> oosVm = new List<Out_of_stock_VM>();
            var All = (from cat in db.categories
                       join itm in db.Items.Where(x => x.QuantityInStock == 0) on cat.Category_ID equals itm.Category_ID
                       select new { cat.Category_Name,itm.ItemCode, itm.Name, itm.QuantityInStock, itm.Description, itm.Image }).ToList();

            foreach (var item in All)
            {
                Out_of_stock_VM model = new Out_of_stock_VM();
                model.itemCode = item.ItemCode;
                model.Name = item.Name;
                model.Category = item.Category_Name;
                model.Image = item.Image;
                model.stockQTY = item.QuantityInStock;
                model.Description = item.Description;
                oosVm.Add(model);
            }
            return (oosVm.ToList());
        }
        public List<Below_Order_Level_VM> Below_Order_Level()
        {
            List<Below_Order_Level_VM> oosVm = new List<Below_Order_Level_VM>();
            var all = db.Items.ToList();
            //dont foget for quantity thats below 0
            foreach (var item in all)
            {
                if (item.QuantityInStock < item.ReOrder_Level)
                {
                    Below_Order_Level_VM model = new Below_Order_Level_VM();
                    model.itemCode = item.ItemCode;
                    model.Name = item.Name;
                    model.Category = item.Categories.Category_Name;
                    model.Image = item.Image;
                    model.stockQTY = item.QuantityInStock;
                    model.Description = item.Description;
                    model.ReOrderLevel = Convert.ToInt32(item.ReOrder_Level);
                    oosVm.Add(model);
                }
            }
            return (oosVm.ToList());
        }

        public void Add_Item_To_PurchaseList(Supplier supplier,int id)
        {
            try
            {
                PurchaseOrder purchase = new PurchaseOrder();
                purchase.SupplierID = supplier.SupplierID;
                purchase.Email = supplier.Email;
                purchase.PurchaseDate = DateTime.Now;


                db.purchaseOrders.Add(purchase);
                db.SaveChanges();
            }
           catch (Exception ex) { }
        }
        public void StockLevels(Item items)
        {
            if (item.QuantityInStock < item.ReOrder_Level ||item.QuantityInStock==0)
            {
                try
                {
                    OutofStockItems outofStock = new OutofStockItems();
                    outofStock.ItemCode = item.ItemCode;
                    outofStock.SupplierId = item.SupplierId;
                    outofStock.Name = item.Name;
                    outofStock.Price = item.Price;
                    outofStock.ReOrder_Level = item.ReOrder_Level;
                    outofStock.ReOrder_Quantity = item.ReOrder_Quantity;
                    outofStock.QuantityInStock = item.QuantityInStock;
                    outofStock.Image = item.Image;


                    db.outofStocks.Add(outofStock);
                    db.SaveChanges();
                }
                catch (Exception ex) { }
            }
           
        }


      
        public void Add_PurchaseOrder_Item(PurchaseOrder order)
        {
            var items = db.Items.ToList();
            foreach (var item in items)
            {
                var id = from c in db.purchaseOrders select c.PurchaseOrderID;
                var icode = from i in db.Items.Where(x => x.QuantityInStock == 0 && x.QuantityInStock < x.ReOrder_Level) select i.ItemCode;
                PurchaseOrderItem purchase = new PurchaseOrderItem();
                purchase.purchaseID = id.FirstOrDefault();
                purchase.itemID = icode.FirstOrDefault();
                purchase.reorderQuantity = item.ReOrder_Quantity;
                purchase.SupplierId = item.SupplierId;
                purchase.status = "waiting for supplier";

                db.purchaseOrderItems.Add(purchase);
                db.SaveChanges();
            }
        }
        public Item findItem_by_id(int? id)
        {
            return db.Items.Find(id);
        }



    }
}