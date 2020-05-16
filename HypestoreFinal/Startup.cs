using HypestoreFinal.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HypestoreFinal.Startup))]
namespace HypestoreFinal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateUserAndRoles();
        }
        public void CreateUserAndRoles()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!roleManager.RoleExists("Admin"))
            {
                //create super admin role
                var role = new IdentityRole("Admin");
                roleManager.Create(role);

                //create default user
                var user = new ApplicationUser();
                user.UserName = "Admin@gmail.com";
                user.Email = "Admin@gmail.com";
                string pwd = "#Password01";

                var newuser = userManager.Create(user, pwd);

                if (newuser.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Admin");

                }
            }
        }
    }
}
