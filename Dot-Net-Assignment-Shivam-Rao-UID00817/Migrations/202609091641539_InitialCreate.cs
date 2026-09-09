namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        address_id = c.Long(nullable: false, identity: true),
                        address_line1 = c.String(nullable: false),
                        address_line2 = c.String(),
                        city = c.String(nullable: false),
                        state = c.String(nullable: false),
                        pincode = c.String(nullable: false),
                        country = c.String(nullable: false),
                        created_at = c.DateTime(nullable: false),
                        updated_at = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.address_id);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        item_id = c.Long(nullable: false, identity: true),
                        name = c.String(nullable: false),
                        price = c.Single(nullable: false),
                        available_quantity = c.Int(nullable: false),
                        created_at = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.item_id);
            
            CreateTable(
                "dbo.Menu_Items",
                c => new
                    {
                        menu_id = c.Long(nullable: false),
                        item_id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.menu_id, t.item_id })
                .ForeignKey("dbo.Items", t => t.item_id, cascadeDelete: true)
                .ForeignKey("dbo.Menus", t => t.menu_id, cascadeDelete: true)
                .Index(t => t.menu_id)
                .Index(t => t.item_id);
            
            CreateTable(
                "dbo.Menus",
                c => new
                    {
                        menu_id = c.Long(nullable: false, identity: true),
                        restaurant_id = c.Long(nullable: false),
                        name = c.String(nullable: false),
                        created_at = c.DateTime(nullable: false),
                        updated_at = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.menu_id)
                .ForeignKey("dbo.Restaurants", t => t.restaurant_id, cascadeDelete: true)
                .Index(t => t.restaurant_id);
            
            CreateTable(
                "dbo.Restaurants",
                c => new
                    {
                        restaurant_id = c.Long(nullable: false, identity: true),
                        name = c.String(nullable: false),
                        created_at = c.DateTime(nullable: false),
                        address_id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.restaurant_id)
                .ForeignKey("dbo.Addresses", t => t.address_id, cascadeDelete: true)
                .Index(t => t.address_id);
            
            CreateTable(
                "dbo.Order_Items",
                c => new
                    {
                        order_id = c.Long(nullable: false),
                        item_id = c.Long(nullable: false),
                        item_price = c.Single(nullable: false),
                        quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.order_id, t.item_id })
                .ForeignKey("dbo.Items", t => t.item_id, cascadeDelete: true)
                .ForeignKey("dbo.Orders", t => t.order_id, cascadeDelete: true)
                .Index(t => t.order_id)
                .Index(t => t.item_id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        order_id = c.Long(nullable: false, identity: true),
                        instructions = c.String(),
                        status = c.Int(nullable: false),
                        address_line1 = c.String(nullable: false),
                        address_line2 = c.String(),
                        city = c.String(nullable: false),
                        state = c.String(nullable: false),
                        pincode = c.Long(nullable: false),
                        country = c.String(nullable: false),
                        created_at = c.DateTime(nullable: false),
                        updated_at = c.DateTime(nullable: false),
                        user_id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.order_id)
                .ForeignKey("dbo.Users", t => t.user_id, cascadeDelete: true)
                .Index(t => t.user_id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        user_id = c.Long(nullable: false, identity: true),
                        email = c.String(maxLength: 255),
                        phone_number = c.String(maxLength: 50),
                        password = c.String(),
                        name = c.String(),
                        role = c.Int(nullable: false),
                        wallet_balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        is_active = c.Boolean(nullable: false),
                        created_at = c.DateTime(nullable: false),
                        updated_at = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.user_id)
                .Index(t => t.email, unique: true, name: "IX _User_Email")
                .Index(t => t.phone_number, unique: true, name: "IX _User_PhoneNumber");
            
            CreateTable(
                "dbo.Owner_Manages_Restaurants",
                c => new
                    {
                        restaurant_id = c.Long(nullable: false),
                        user_id = c.Long(nullable: false),
                        created_at = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.restaurant_id, t.user_id })
                .ForeignKey("dbo.Restaurants", t => t.restaurant_id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.user_id, cascadeDelete: true)
                .Index(t => t.restaurant_id)
                .Index(t => t.user_id);
            
            CreateTable(
                "dbo.User_Address_Type",
                c => new
                    {
                        user_id = c.Long(nullable: false),
                        address_id = c.Long(nullable: false),
                        address_type = c.String(),
                    })
                .PrimaryKey(t => new { t.user_id, t.address_id })
                .ForeignKey("dbo.Addresses", t => t.address_id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.user_id, cascadeDelete: true)
                .Index(t => t.user_id)
                .Index(t => t.address_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.User_Address_Type", "user_id", "dbo.Users");
            DropForeignKey("dbo.User_Address_Type", "address_id", "dbo.Addresses");
            DropForeignKey("dbo.Owner_Manages_Restaurants", "user_id", "dbo.Users");
            DropForeignKey("dbo.Owner_Manages_Restaurants", "restaurant_id", "dbo.Restaurants");
            DropForeignKey("dbo.Order_Items", "order_id", "dbo.Orders");
            DropForeignKey("dbo.Orders", "user_id", "dbo.Users");
            DropForeignKey("dbo.Order_Items", "item_id", "dbo.Items");
            DropForeignKey("dbo.Menu_Items", "menu_id", "dbo.Menus");
            DropForeignKey("dbo.Menus", "restaurant_id", "dbo.Restaurants");
            DropForeignKey("dbo.Restaurants", "address_id", "dbo.Addresses");
            DropForeignKey("dbo.Menu_Items", "item_id", "dbo.Items");
            DropIndex("dbo.User_Address_Type", new[] { "address_id" });
            DropIndex("dbo.User_Address_Type", new[] { "user_id" });
            DropIndex("dbo.Owner_Manages_Restaurants", new[] { "user_id" });
            DropIndex("dbo.Owner_Manages_Restaurants", new[] { "restaurant_id" });
            DropIndex("dbo.Users", "IX _User_PhoneNumber");
            DropIndex("dbo.Users", "IX _User_Email");
            DropIndex("dbo.Orders", new[] { "user_id" });
            DropIndex("dbo.Order_Items", new[] { "item_id" });
            DropIndex("dbo.Order_Items", new[] { "order_id" });
            DropIndex("dbo.Restaurants", new[] { "address_id" });
            DropIndex("dbo.Menus", new[] { "restaurant_id" });
            DropIndex("dbo.Menu_Items", new[] { "item_id" });
            DropIndex("dbo.Menu_Items", new[] { "menu_id" });
            DropTable("dbo.User_Address_Type");
            DropTable("dbo.Owner_Manages_Restaurants");
            DropTable("dbo.Users");
            DropTable("dbo.Orders");
            DropTable("dbo.Order_Items");
            DropTable("dbo.Restaurants");
            DropTable("dbo.Menus");
            DropTable("dbo.Menu_Items");
            DropTable("dbo.Items");
            DropTable("dbo.Addresses");
        }
    }
}
