using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HypestoreFinal.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string Full_Name { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> categories  { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> orderItems { get; set; }
        public DbSet<OrderTracking> orderTrackings { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<OrderAddress> orderAddresses { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<PurchaseOrder> purchaseOrders { get; set; }
        public DbSet<PurchaseOrderItem> purchaseOrderItems { get; set; }

        public DbSet<OutofStockItems> outofStocks { get; set; }

        public DbSet<IdentityUserRole> UserInRole { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

    
    }
}