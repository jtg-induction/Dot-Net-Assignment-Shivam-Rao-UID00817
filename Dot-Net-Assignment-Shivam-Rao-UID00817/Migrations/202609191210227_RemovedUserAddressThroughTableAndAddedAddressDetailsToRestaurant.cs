namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedUserAddressThroughTableAndAddedAddressDetailsToRestaurant : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Restaurants", "address_id", "dbo.Addresses");
            DropForeignKey("dbo.User_Address_Type", "address_id", "dbo.Addresses");
            DropForeignKey("dbo.User_Address_Type", "user_id", "dbo.Users");
            DropIndex("dbo.Restaurants", new[] { "address_id" });
            DropIndex("dbo.User_Address_Type", new[] { "user_id" });
            DropIndex("dbo.User_Address_Type", new[] { "address_id" });
            AddColumn("dbo.Addresses", "user_id", c => c.Long(nullable: false));
            AddColumn("dbo.Restaurants", "address_line1", c => c.String(nullable: false));
            AddColumn("dbo.Restaurants", "address_line2", c => c.String());
            AddColumn("dbo.Restaurants", "city", c => c.String(nullable: false));
            AddColumn("dbo.Restaurants", "state", c => c.String(nullable: false));
            AddColumn("dbo.Restaurants", "pincode", c => c.String(nullable: false, maxLength: 12));
            AddColumn("dbo.Restaurants", "country", c => c.String(nullable: false));
            AddColumn("dbo.Restaurants", "updated_at", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Addresses", "pincode", c => c.String(nullable: false, maxLength: 12));
            AlterColumn("dbo.Orders", "pincode", c => c.String(nullable: false, maxLength: 12));
            CreateIndex("dbo.Addresses", "user_id");
            AddForeignKey("dbo.Addresses", "user_id", "dbo.Users", "user_id", cascadeDelete: false);
            DropColumn("dbo.Restaurants", "address_id");
            DropTable("dbo.User_Address_Type");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.User_Address_Type",
                c => new
                    {
                        user_id = c.Long(nullable: false),
                        address_id = c.Long(nullable: false),
                        address_type = c.String(nullable: false),
                    })
                .PrimaryKey(t => new { t.user_id, t.address_id });
            
            AddColumn("dbo.Restaurants", "address_id", c => c.Long(nullable: false));
            DropForeignKey("dbo.Addresses", "user_id", "dbo.Users");
            DropIndex("dbo.Addresses", new[] { "user_id" });
            AlterColumn("dbo.Orders", "pincode", c => c.String(nullable: false, maxLength: 6));
            AlterColumn("dbo.Addresses", "pincode", c => c.String(nullable: false, maxLength: 6));
            DropColumn("dbo.Restaurants", "updated_at");
            DropColumn("dbo.Restaurants", "country");
            DropColumn("dbo.Restaurants", "pincode");
            DropColumn("dbo.Restaurants", "state");
            DropColumn("dbo.Restaurants", "city");
            DropColumn("dbo.Restaurants", "address_line2");
            DropColumn("dbo.Restaurants", "address_line1");
            DropColumn("dbo.Addresses", "user_id");
            CreateIndex("dbo.User_Address_Type", "address_id");
            CreateIndex("dbo.User_Address_Type", "user_id");
            CreateIndex("dbo.Restaurants", "address_id");
            AddForeignKey("dbo.User_Address_Type", "user_id", "dbo.Users", "user_id", cascadeDelete: false);
            AddForeignKey("dbo.User_Address_Type", "address_id", "dbo.Addresses", "address_id", cascadeDelete: false);
            AddForeignKey("dbo.Restaurants", "address_id", "dbo.Addresses", "address_id", cascadeDelete: false);
        }
    }
}
