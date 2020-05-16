using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HypestoreFinal.Models;
using HypestoreFinal.Helpers;
using HypestoreFinal.ViewModels;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace HypestoreFinal.Controllers
{
    public class CustomersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        Customer_Business cb = new Customer_Business();


        ApplicationDbContext con = new ApplicationDbContext();
        public CustomersController()
        {
        }

        public CustomersController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }
        public ActionResult Index()
        {
            if (User.IsInRole(WebConstants.CustomerRole))
            {
                var owner = HttpContext.User.Identity.Name;
                var user = db.Customers.FirstOrDefault(s => s.Email == owner);
                if (user == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    //var doc = db.Doctors.ToList().Find(x => x.Email == HttpContext.User.Identity.Name).DoctorId;

                    return View(cb.all().Where(x => x.Email == user.Email));
                }
            }
            else
                return View(cb.all());
        }

        //public ActionResult MyIndex(int? id, int? PetID, int? visitID)
        //{
        //    var viewModel = new CustomerIndexData();

        //    viewModel.Customers = db.Customers
        //        .OrderBy(i => i.LastName);

        //    if (id != null)
        //    {
        //        ViewBag.CustomerID = id.Value;
        //        //viewModel.Pets = viewModel.Customers.Where(
        //         //   i => i.CustomerId == id.Value).Single().Pets;
        //    }

        //    if (PetID != null)
        //    {
        //        ViewBag.PetID = PetID.Value;
        //        // Lazy loading
        //        //viewModel.Enrollments = viewModel.Courses.Where(
        //        //    x => x.CourseID == courseID).Single().Enrollments;
        //        // Explicit loading
        //        var selectedPet = viewModel.Pets.Where(x => x.PetId == PetID).Single();
        //        db.Entry(selectedPet).Collection(x => x.PetVisits).Load();
        //        foreach (PetVisit enrollment in selectedPet.PetVisits)
        //        {
        //            db.Entry(enrollment).Reference(x => x.Patient).Load();
        //        }

        //        viewModel.PetVisits = selectedPet.PetVisits;
        //    }
        //    if (visitID != null)
        //    {
        //        ViewBag.VD = visitID.Value;
        //        var selectedPetVisit = db.PetVisits.Where(x => x.VisitId == visitID).Single();
        //        db.Entry(selectedPetVisit).Collection(x => x.Prescriptions).Load();
        //        foreach (Prescription prescr in selectedPetVisit.Prescriptions)
        //        {
        //            db.Entry(prescr).Reference(x => x.PetVisit).Load();
        //        }
        //        viewModel.prescriptions = selectedPetVisit.Prescriptions;
        //    }

        //    return View(viewModel);
        //}
        [Authorize]
        public ActionResult Summary()
        {
            var me = User.Identity.Name;
            List<SummaryVM> svm = new List<SummaryVM>();
            //var join = (from cu in db.Customers.ToList().Where(x => x.Email == HttpContext.User.Identity.Name)
            //            join pt in db.Pets on cu.CustomerId equals pt.OwnerEmail
            //            join vs in db.PetVisits on pt.PetId equals vs.PetId
            //            join pres in db.Prescriptions on vs.VisitId equals pres.VisitId

                        //select new
                        //{
                        //    cu.FirstName,
                        //    pt.pet_Name,
                        //    vs.Date,
                        //    vs.PrimaryDiagnosis,
                        //    pres.Price

                        //}).ToList();

            //foreach (var item in join)
            //{
            //    SummaryVM model = new SummaryVM();
            //    model.CustomerName = item.FirstName;
            //    model.Diagnosis = item.PrimaryDiagnosis;
            //    model.Price = item.Price;
            //    model.PetName = item.pet_Name;

            //    model.VisitDate = item.Date;
            //    svm.Add(model);
            //}


            return View(svm.ToList());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
                return RedirectToAction("Bad_Request", "Error");
            if (cb.find_by_id(id) != null)
                return View(cb.find_by_id(id));
            else
                return RedirectToAction("Not_Found", "Error");
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CustomerCreateVM model)
        {
            RolesBusiness rb = new RolesBusiness();

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, PhoneNumber = model.phone, EmailConfirmed = true };
                var adminresult = await UserManager.CreateAsync(user, model.Password);

                //cb.add(model);

                if (adminresult.Succeeded)
                {
                    ApplicationDbContext db = new ApplicationDbContext();
                    rb.AddUserToRole(db.Users.FirstOrDefault(x => x.UserName == model.Email).Id, WebConstants.CustomerRole);

                    // await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code }, protocol: Request.Url.Scheme);
                    var client = new SendGridClient("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");
                    var from = new EmailAddress("no-reply@MontclairOnlineStore.com", "MontClair Veterinary");
                    var subject = "Confirm your account";
                    var to = new EmailAddress(model.Email, model.FirstName + " " + model.LastName);
                    var htmlContent = "Hello " + model.FirstName + " " + model.LastName + ", Kindly confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>";
                    var msg = MailHelper.CreateSingleEmail(from, to, subject, null, htmlContent);
                    var response = client.SendEmailAsync(msg);

                }

                //Success(string.Format("<b>{0}</b> was successfully added to the database.", model.FirstName), true);
                return RedirectToAction("Create");
            }

            return View(model);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("Bad_Request", "Error");
            if (cb.find_by_id(id) != null)
                return View(cb.find_by_id(id));
            else
                return RedirectToAction("Not_Found", "Error");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Customer model)
        {
            if (ModelState.IsValid)
            {
                cb.edit(model);
                //Success(string.Format("<b>{0}</b> was successfully updated.", model.FirstName), true);
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}