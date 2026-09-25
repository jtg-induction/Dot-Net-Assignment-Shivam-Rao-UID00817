namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddedRestaurantIdToOrderesAndIndexes : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Addresses", new[] { "user_id" });
            DropIndex("dbo.Items", new[] { "restaurant_id" });
            AddColumn("dbo.Orders", "restaurant_id", c => c.Long(nullable: false));
            CreateIndex("dbo.Addresses", "user_id", name: "IX_User_Id");
            CreateIndex("dbo.Orders", "restaurant_id", name: "IX_Restaurant_Id");
            CreateIndex("dbo.Items", "restaurant_id", name: "IX_Restaurant_Id");
            AddForeignKey("dbo.Orders", "restaurant_id", "dbo.Restaurants", "restaurant_id", cascadeDelete: true);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Orders", "restaurant_id", "dbo.Restaurants");
            DropIndex("dbo.Items", "IX_Restaurant_Id");
            DropIndex("dbo.Orders", "IX_Restaurant_Id");
            DropIndex("dbo.Addresses", "IX_User_Id");
            DropColumn("dbo.Orders", "restaurant_id");
            CreateIndex("dbo.Items", "restaurant_id");
            CreateIndex("dbo.Addresses", "user_id");
        }
    }
}
