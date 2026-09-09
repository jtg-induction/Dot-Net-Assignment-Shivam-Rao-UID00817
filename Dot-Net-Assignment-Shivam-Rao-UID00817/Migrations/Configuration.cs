namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<Dot_Net_Assignment_Shivam_Rao_UID00817.Restaurant_ManagementContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Dot_Net_Assignment_Shivam_Rao_UID00817.Restaurant_ManagementContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
