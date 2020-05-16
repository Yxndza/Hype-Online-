using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using HypestoreFinal.Models;
using System.Net.Mail;

namespace HypestoreFinal.Controllers
{
    public class ShoppingController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public string shoppingCartID { get; set; }
       
        public const string CartSessionKey = "CartId";

        public ActionResult Index()
        {
            return View(db.Items.ToList());
        }
        public ActionResult Index2()
        {
            return View(db.Items.ToList());
        }

        //private ActionResult View(object p)
        //{
        //    throw new NotImplementedException();
        //}

        public ActionResult Addtocart(int id, CartItem model)
        {
            var item = db.Items.Find(id);
            if (item != null)
            {
                Additemtocart(id);

                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Not_Found", "Error");
            }
        }
        public ActionResult ShoppingCart()
        {
            shoppingCartID = GetCartID();
            ViewBag.Total = getcarttotal(id: shoppingCartID);
            ViewBag.TotalQTY = getCartItems().FindAll(x => x.cartid == shoppingCartID).Sum(q => q.quantity);
            return View(db.CartItems.ToList().FindAll(x => x.cartid == shoppingCartID));
        }

        [HttpPost]
        public ActionResult ShoppingCart(List<CartItem> items)
        {
            shoppingCartID = GetCartID();

            foreach (var i in items)
            {
                updateCart(i.cartitemid, i.quantity);
            }
            ViewBag.Total = getcarttotal(shoppingCartID);
            ViewBag.TotalQTY = getCartItems().FindAll(x => x.cartid == shoppingCartID).Sum(q => q.quantity);
            return View(db.CartItems.ToList().FindAll(x => x.cartid == shoppingCartID));
        }
        public ActionResult countCartItems()
        {
            int qty = getCartItems().Count();
            return Content(qty.ToString());
        }
        public ActionResult Checkout()
        {
            if (getCartItems().Count == 0)
            {
                ViewBag.Err = "Opps... you should have atleast one cart item, please shop a few items";
                return RedirectToAction("Index");
            }
            else
                return RedirectToAction("DeliveryOption");
        }
        //  [Authorize]
        public ActionResult DeliveryOption()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DeliveryOption(string colorRadio, string Street, string City, string PostalCode)
        {
            if (!String.IsNullOrEmpty(colorRadio))
            {
                if (colorRadio.Equals("StandardDelivery"))
                {

                    Session["Street"] = Street;
                    Session["City"] = City;
                    Session["PostalCode"] = PostalCode;
                    return RedirectToAction("PlaceOrder", new { id = "deliver" });
                }
            }
            return View();
        }
        public ActionResult PlaceOrder(string id, Customer customer)
        {
            var Customer = db.Customers.ToList().Find(x => x.Email == HttpContext.User.Identity.Name);
            db.Orders.Add(new Order()
            {
                OrderID = GenerateOrderNumber(5),
                Email = customer.Email,
                date_created = DateTime.Now,
                shipped = false,
                customerId = Customer.CustomerId,
                status = "Awaiting Payment"
            });
            db.SaveChanges();
            var order = db.Orders.ToList()
                .FindAll(x => x.Email == customer.Email)
                .LastOrDefault();

            if (id == "deliver")
            {
                db.orderAddresses.Add(new OrderAddress()
                {
                    OrderIDs = order.OrderID,
                    street = Session["Street"].ToString(),
                    city = Session["City"].ToString(),
                    zipcode = Session["PostalCode"].ToString()
                });
                db.SaveChanges();
            }

            var items = getCartItems();

            foreach (var item in items)
            {
                var x = new OrderItem()
                {
                    OrderId = order.OrderID,
                    ItemId = item.ItemId,
                    Quantity = item.quantity,
                    Price = item.price
                };
                db.orderItems.Add(x);
                db.SaveChanges();
            }
            emptyCart();

            db.SaveChanges();

            //Redirect to payment
            return RedirectToAction("Payment", new { id = order.OrderID });
        }
        [Authorize]
        public ActionResult Payment(string id)
        {
            var order = db.Orders.Find(id);
            ViewBag.Order = order;
            ViewBag.Account = db.Customers.Find(order.customerId);
            ViewBag.Address = db.orderAddresses.ToList().Find(x => x.OrderIDs == order.OrderID);
            ViewBag.Items = db.orderItems.ToList().FindAll(x => x.OrderId == order.OrderID);
            ViewBag.Total = getordertotal(Convert.ToInt32(order.OrderID));


            try
            {
                string url = "<a href=" + "http://hypestore.azurewebsites.net//Shopping/Payment/Shopping/Payment/" + id + " >  here" + "</a>";
                string table = "<br/>" +
                               "Items in this order<br/>" +
                               "<table>";
                table += "<tr>" +
                         "<th>Item</th>"
                         +
                         "<th>Quantity</th>"
                         +
                         "<th>Price</th>" +
                         "</tr>";
                foreach (var item in (List<OrderItem>)ViewBag.Items)
                {
                    string itemsinoder = "<tr> " +
                                         "<td>" + item.Item.Name + " </td>" +
                                         "<td>" + item.Quantity + " </td>" +
                                         "<td>R " + item.Price + " </td>" +
                                         "<tr/>";
                    table += itemsinoder;
                }

                table += "<tr>" +
                         "<th></th>"
                         +
                         "<th></th>"
                         +
                         "<th>" + getordertotal(Convert.ToInt32(order.OrderID)).ToString("R0.00") + "</th>" +
                         "</tr>";
                table += "</table>";

                var client = new SendGridClient("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");
                var from = new EmailAddress("no-reply@MontclairOnlineStore.com", "MontClair Veterinary");
                var subject = "transaction " + id + " | Awaiting Payment";
                var to = new EmailAddress(((Customer)ViewBag.Account).Email, ((Customer)ViewBag.Account).FirstName + " " + ((Customer)ViewBag.Account).LastName);
                var htmlContent = "Hi " + order.Customer.FirstName + ", Your transaction is being processed, you can securely pay for your purchase from " + url + table;
                var msg = MailHelper.CreateSingleEmail(from, to, subject, null, htmlContent);
                var response = client.SendEmailAsync(msg);
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        [Authorize]
        public ActionResult SecurePayment(string id)
        {
            var order = db.Orders.Find(id);
            ViewBag.Order = order;
            ViewBag.Account = db.Customers.Find(order.customerId);
            ViewBag.Address = db.orderAddresses.ToList().Find(x => x.OrderIDs == order.OrderID);
            ViewBag.Items = db.orderItems.ToList().FindAll(x => x.OrderId == order.OrderID);

            ViewBag.Total = getordertotal(Convert.ToInt32(order.OrderID));

            return Redirect(PaymentLink(getordertotal(Convert.ToInt32(order.OrderID)).ToString(), "Purchase Payment | Transaction No: " + order.OrderID, Convert.ToInt32(order.OrderID)));
        }
        public ActionResult Payment_Cancelled(string id)
        {
            var order = db.Orders.Find(id);
            ViewBag.Order = order;
            ViewBag.Account = db.Customers.Find(order.customerId);
            ViewBag.Address = db.orderAddresses.ToList().Find(x => x.OrderIDs == order.OrderID);
            ViewBag.Items = db.orderItems.ToList().FindAll(x => x.OrderId == order.OrderID);

            ViewBag.Total = getordertotal(Convert.ToInt32(order.OrderID));
            try
            {
                string url = "<a href=" + "http://hypestore.azurewebsites.net/Shopping/Payment/" + id + " >  here" + "</a>";
                string table = "<br/>" +
                               "Items in this order<br/>" +
                               "<table>";
                table += "<tr>" +
                         "<th>Item</th>"
                         +
                         "<th>Quantity</th>"
                         +
                         "<th>Price</th>" +
                         "</tr>";
                foreach (var item in (List<OrderItem>)ViewBag.Items)
                {
                    string items = "<tr> " +
                                   "<td>" + item.Item.Name + " </td>" +
                                   "<td>" + item.Quantity + " </td>" +
                                   "<td>R " + item.Price + " </td>" +
                                   "<tr/>";
                    table += items;
                }

                table += "<tr>" +
                         "<th></th>"
                         +
                         "<th></th>"
                         +
                         "<th>" + getordertotal(Convert.ToInt32(order.OrderID)).ToString("R0.00") + "</th>" +
                         "</tr>";
                table += "</table>";

                var client = new SendGridClient("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");
                var from = new EmailAddress("no-reply@MontclairOnlineStore.com", "MontClair Veterinary");
                var subject = "Order " + id + " | Awaiting Payment";
                var to = new EmailAddress(order.Customer.Email, order.Customer.FirstName + " " + order.Customer.LastName);
                var htmlContent = "Hi " + order.Customer.FirstName + ", Your order payment process was cancelled, you can still securely pay for your purchase from " + url + table;
                var msg = MailHelper.CreateSingleEmail(from, to, subject, null, htmlContent);
                var response = client.SendEmailAsync(msg);
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        public ActionResult PaymentSuccessfull(int? id)
        {
            var order = db.Orders.Find(id);
            try
            {


                db.Payments.Add(new Payment()
                {
                    Date = DateTime.Now,
                    Email = db.Customers.FirstOrDefault(p => p.Email == User.Identity.Name).Email,
                    AmountPaid = getordertotal(Convert.ToInt32(order.OrderID)),
                    PaymentFor = "Transaction " + id + " Payment",
                    PaymentMethod = "PayFast Online"
                });
                db.SaveChanges();
                ViewBag.Items = db.orderItems.ToList().FindAll(x => x.OrderId == order.OrderID);

                update_Stock((int)id);

                string table = "<br/>" +
                               "Items in this order<br/>" +
                               "<table>";
                table += "<tr>" +
                         "<th>Item</th>"
                         +
                         "<th>Quantity</th>"
                         +
                         "<th>Price</th>" +
                         "</tr>";
                foreach (var item in (List<OrderItem>)ViewBag.Items)
                {
                    string items = "<tr> " +
                                   "<td>" + item.Item.Name + " </td>" +
                                   "<td>" + item.Quantity + " </td>" +
                                   "<td>R " + item.Price + " </td>" +
                                   "<tr/>";
                    table += items;
                }

                table += "<tr>" +
                         "<th></th>"
                         +
                         "<th></th>"
                         +
                         "<th>" + getordertotal(Convert.ToInt32(order.OrderID)).ToString("R 0.00") + "</th>" +
                         "</tr>";
                table += "</table>";

                var client = new SendGridClient("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");
                var from = new EmailAddress("no-reply@MontclairOnlineStore.com", "MontClair Veterinary");
                var subject = "Order " + id + " | Payment Recieved";
                var to = new EmailAddress(order.Customer.Email, order.Customer.FirstName + " " + order.Customer.LastName);
                var htmlContent = "Hi " + order.Customer.FirstName + ", We recieved your payment, you will have your goodies any time from now " + table;
                var msg = MailHelper.CreateSingleEmail(from, to, subject, null, htmlContent);
                var response = client.SendEmailAsync(msg);
            }
            catch (Exception ex) { }

            ViewBag.Order = order;
            ViewBag.Account = db.Customers.Find(order.customerId);
            ViewBag.Address = db.orderAddresses.ToList().Find(x => x.OrderIDs == order.OrderID);
            ViewBag.Total = getordertotal(Convert.ToInt32(order.OrderID));

            return View();
        }
        #region Cart Methods
        public void Additemtocart(int id)
        {
            shoppingCartID = GetCartID();

            var item = db.Items.Find(id);
            if (item != null)
            {
                var cartItem =
                    db.CartItems.FirstOrDefault(x => x.cartid == shoppingCartID && x.ItemId == item.ItemCode);
                if (cartItem == null)
                {
                    var cart = db.Carts.Find(shoppingCartID);
                    if (cart == null)
                    {
                        db.Carts.Add(entity: new Cart()
                        {
                            cart_id = shoppingCartID,
                            date_created = DateTime.Now
                        });
                        db.SaveChanges();
                    }

                    db.CartItems.Add(entity: new CartItem()
                    {
                        cartitemid = Guid.NewGuid().ToString(),
                        cartid = shoppingCartID,
                        ItemId = item.ItemCode,
                        quantity = 1,
                        price = item.Price
                    }
                        );
                }
                else
                {
                    cartItem.quantity++;
                }
                db.SaveChanges();
            }
        }
        public void remove_item_from_cart(string id)
        {
            shoppingCartID = GetCartID();

            var item = db.CartItems.Find(id);
            if (item != null)
            {
                var cartItem =
                    db.CartItems.FirstOrDefault(predicate: x => x.cartid == shoppingCartID && x.ItemId == item.ItemId);
                if (cartItem != null)
                {
                    db.CartItems.Remove(entity: cartItem);
                }
                db.SaveChanges();
            }
        }
        public List<CartItem> getCartItems()
        {
            shoppingCartID = GetCartID();
            return db.CartItems.ToList().FindAll(match: x => x.cartid == shoppingCartID);
        }
        public void updateCart(string id, int qty)
        {
            var item = db.CartItems.Find(id);
            if (qty < 0)
                item.quantity = qty / -1;
            else if (qty == 0)
                remove_item_from_cart(item.cartitemid);
            else
                item.quantity = qty;
            db.SaveChanges();
        }
        public double getcarttotal(string id)
        {
            double amount = 0;
            foreach (var item in db.CartItems.ToList().FindAll(match: x => x.cartid == id))
            {
                amount += (item.price * item.quantity);
            }
            return amount;
        }
        public void emptyCart()
        {
            shoppingCartID = GetCartID();
            foreach (var item in db.CartItems.ToList().FindAll(match: x => x.cartid == shoppingCartID))
            {
                db.CartItems.Remove(item);
            }
            try
            {
                db.Carts.Remove(db.Carts.Find(shoppingCartID));
                db.SaveChanges();
            }
            catch (Exception ex) { }
        }
        public string GetCartID()
        {
            if (System.Web.HttpContext.Current.Session[name: CartSessionKey] == null)
            {
                if (!String.IsNullOrWhiteSpace(value: System.Web.HttpContext.Current.User.Identity.Name))
                {
                    System.Web.HttpContext.Current.Session[name: CartSessionKey] = System.Web.HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    Guid temp = Guid.NewGuid();
                    System.Web.HttpContext.Current.Session[name: CartSessionKey] = temp.ToString();
                }
            }
            return System.Web.HttpContext.Current.Session[name: CartSessionKey].ToString();
        }
        #endregion

        #region Customer Order Methods
        public double getordertotal(int id)
        {
            double amount = 0;
            foreach (var item in db.orderItems.ToList().FindAll(match: x => Convert.ToInt32(x.OrderId) == id))
            {
                amount += (item.Price * item.Quantity);
            }
            return amount;
        }
        public string PaymentLink(string totalCost, string paymentSubjetc, int order_id)
        {

            string paymentMode = ConfigurationManager.AppSettings["PaymentMode"], site, merchantId, merchantKey, returnUrl;


            {
                site = "https://sandbox.payfast.co.za/eng/process?";
                merchantId = "10002201";
                merchantKey = "25lbpwmazv8rn";
            }

            var stringBuilder = new StringBuilder();


            stringBuilder.Append("&merchant_id=" + HttpUtility.HtmlEncode(merchantId));
            stringBuilder.Append("&merchant_key=" + HttpUtility.HtmlEncode(merchantKey));
            stringBuilder.Append("&return_url=" + HttpUtility.HtmlEncode("http://hypestore.azurewebsites.net/Shopping/Payment/Shopping/Payment_Successfull/" + order_id));
            stringBuilder.Append("&cancel_url=" + HttpUtility.HtmlEncode("http://hypestore.azurewebsites.net/Shopping/Payment/Payment_Cancelled/" + order_id));
            stringBuilder.Append("&notify_url=" + HttpUtility.HtmlEncode(ConfigurationManager.AppSettings["PF_NotifyURL"]));

            string amt = totalCost;
            amt = amt.Replace(",", ".");

            stringBuilder.Append("&amount=" + HttpUtility.HtmlEncode(amt));
            stringBuilder.Append("&item_name=" + HttpUtility.HtmlEncode(paymentSubjetc));
            stringBuilder.Append("&email_confirmation=" + HttpUtility.HtmlEncode("1"));
            stringBuilder.Append("&confirmation_address=" + HttpUtility.HtmlEncode(ConfigurationManager.AppSettings["PF_ConfirmationAddress"]));

            return (site + stringBuilder);
        }
        public void update_Stock(int id)
        {
            var order = db.Orders.Find(id);
            List<OrderItem> items = db.orderItems.ToList().FindAll(x => Convert.ToInt32(x.OrderId) == id);
            foreach (var item in items)
            {
                var product = db.Items.Find(item.ItemId);
                if (product != null)
                {
                    if ((product.QuantityInStock -= item.Quantity) >= 0)
                    {
                        product.QuantityInStock -= item.Quantity;
                    }
                    else
                    {
                        item.Quantity = product.QuantityInStock;
                        product.QuantityInStock = 0;
                    }
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception ex) { }
                }
            }
        }
        public string GenerateOrderNumber(int length)
        {
            var ra = new Random();
            string number = "";
            for (int i = 0; i < length; i++)
                number = String.Concat(number, ra.Next(5).ToString());
            return number;

        }
        #endregion


    }
}