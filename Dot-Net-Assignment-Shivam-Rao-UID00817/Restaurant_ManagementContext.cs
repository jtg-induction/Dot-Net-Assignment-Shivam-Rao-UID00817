using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using System.Data.Entity;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817
{
    public class Restaurant_ManagementContext : DbContext
    {
        public Restaurant_ManagementContext() : base("name=Restaurant_ManagementContext")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<Restaurant_ManagementContext>());
        }
        public DbSet<Users> Users { get; set; }
        public DbSet<Addresses> Addresses { get; set; }
        public DbSet<User_Address_Type> User_Address_Type { get; set; }
        public DbSet<Restaurants> Restaurants { get; set; }
        public DbSet<Owner_Manages_Restaurants> owner_Manages_Restaurants { get; set; }
        public DbSet<Menus> Menus { get; set; }
        public DbSet<Items> Items { get; set; }
        public DbSet<Menu_Items> Menu_Items { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<Order_Items> Order_Items { get; set; }
    }
}
