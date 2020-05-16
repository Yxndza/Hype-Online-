using System.Linq;
using System.Web.Mvc;
using HypestoreFinal.Models;
using HypestoreFinal.ViewModels;
using System;

namespace HypestoreFinal.Controllers
{
    public class SubscriptionController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Subscription
        public ActionResult Index()
        {
            return View(db.Subscriptions.ToList());
        }

        // GET: Subscription/SignUp
        public ActionResult SignUp()
        {
            return View(new Subscription());
        }

        // POST: Subscription/SignUp
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp([Bind(Include = "ID,EmailAddress,Source,SourceOther,Reason")] Subscription subscription)
        {
            subscription.EmailAddress = subscription.EmailAddress.Trim();

            //Check existing email address
            if (db.Subscriptions.Any(s => s.EmailAddress == subscription.EmailAddress))
            {
                ViewBag.CssClass = "danger";
                ViewBag.Message = "Your email address is already signed up";
            }
            else if (ModelState.IsValid)
            {
                subscription.DateTime = DateTime.Now;
                db.Subscriptions.Add(subscription);
                db.SaveChanges();
                ViewBag.CssClass = "success";
                ViewBag.Message = "You have been succesfully signed up!";
                ModelState.Clear();
                return View();
            }

            return View(subscription);
        }
                
        // POST: Subscription/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Subscription subscription = db.Subscriptions.Find(id);
            db.Subscriptions.Remove(subscription);
            db.SaveChanges();
            return View("Index", db.Subscriptions.ToList());
        }

        // GET: Subscription/Report
        public ActionResult Report()
        {
            HypeProjectReport report = new HypeProjectReport(db.Subscriptions.ToList());
            return View(report);
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
