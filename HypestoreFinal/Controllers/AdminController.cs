using HypestoreFinal.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HypestoreFinal.Controllers
{
    public class AdminController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CreateUser()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateUser(FormCollection form)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            string Username = form["txtEmail"];
            string email = form["txtEmail"];
            string pwd = form["txtPassword"];

            //create default user
            var user = new ApplicationUser();
            user.UserName = Username;
            user.Email = email;


            var newuser = userManager.Create(user, pwd);


            return View();
        }
        public ActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public ActionResult NewRole(FormCollection form)
        {
            string rolename = form["RoleName"];
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!roleManager.RoleExists(rolename))
            {
                //create super admin role
                var role = new IdentityRole(rolename);
                roleManager.Create(role);
            }
            return View("Index");
        }
        public ActionResult AssignRole()
        {
            ViewBag.Roles = context.Roles.Select(r => new SelectListItem { Value = r.Name, Text = r.Name }).ToList();
            return View();
        }
        [HttpPost]
        public ActionResult AssignRole(FormCollection form)
        {
            string usrname = form["txtUserName"];
            string rolname = form["RoleName"];
            ApplicationUser user = context.Users.Where(u => u.UserName.Equals(usrname, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            userManager.AddToRole(user.Id, rolname);

            return View("Index");
        }

        [HttpGet]
        public ActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete(string RoleName)
        {
            var thisRole = context.Roles.Where(r => r.Name.Equals(RoleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            context.Roles.Remove(thisRole);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult UsersAssigned()
        {
            var usersRoles = (from user in context.Users
                              from userRole in user.Roles
                              join role in context.Roles on userRole.RoleId equals
                              role.Id
                              select new UserViewModel()
                              {
                                  Username = user.UserName,
                                  RoleName = role.Name
                              }).ToList();
            return View(usersRoles);
        }

        public ActionResult Roles()
        {
            var roles = context.Roles.ToList();
            return View(roles);
        }
        public ActionResult Users()
        {
            var users = (from user in context.Users
                         select new UserViewModel()
                         {
                             Username = user.UserName,
                         }).ToList();
            return View(users);
        }
    }
}