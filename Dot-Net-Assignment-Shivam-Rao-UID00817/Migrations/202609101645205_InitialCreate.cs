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
                        pincode = c.String(nullable: false, maxLength: 6),
                        country = c.String(nullable: false),
                        created_at = c.DateTime(nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                        updated_at = c.DateTime(nullable: false, defaultValueSql: "SYSUTCDATETIME()") ,
                    })
                .PrimaryKey(t => t.address_id);
            
            CreateTable(
                "dbo.Restaurants",
                c => new
                    {
                        restaurant_id = c.Long(nullable: false, identity: true),
                        name = c.String(nullable: false),
                        created_at = c.DateTime(nullable: false),
                        address_id = c.Long(nullable: false),
                        is_active = c.Boolean(nullable: false, defaultValue: true),
                    })
                .PrimaryKey(t => t.restaurant_id)
                .ForeignKey("dbo.Addresses", t => t.address_id, cascadeDelete: false)
                .Index(t => t.address_id);
            
            CreateTable(
                "dbo.Menus",
                c => new
                    {
                        menu_id = c.Long(nullable: false, identity: true),
                        restaurant_id = c.Long(nullable: false),
                        name = c.String(nullable: false),
                        is_active = c.Boolean(nullable: false, defaultValue: true) ,
                        created_at = c.DateTime(nullable: false, defaultValueSql: "SYSUTCDATETIME()") ,
                        updated_at = c.DateTime(nullable: false, defaultValueSql: "SYSUTCDATETIME()") ,
                    })
                .PrimaryKey(t => t.menu_id)
                .ForeignKey("dbo.Restaurants", t => t.restaurant_id, cascadeDelete: false)
                .Index(t => t.restaurant_id);
            
            CreateTable(
                "dbo.Menu_Items",
                c => new
                    {
                        menu_id = c.Long(nullable: false),
                        item_id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.menu_id, t.item_id })
                .ForeignKey("dbo.Items", t => t.item_id, cascadeDelete: false)
                .ForeignKey("dbo.Menus", t => t.menu_id, cascadeDelete: false)
                .Index(t => t.menu_id)
                .Index(t => t.item_id);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        item_id = c.Long(nullable: false, identity: true),
                        name = c.String(nullable: false),
                        price = c.Decimal(nullable: false, precision: 11, scale: 2),
                        available_quantity = c.Int(nullable: false),
                        is_active = c.Boolean(nullable: false, defaultValue: true) ,
                        created_at = c.DateTime(nullable: false, defaultValueSql: "SYSUTCDATETIME()") ,
                    })
                .PrimaryKey(t => t.item_id);

            Sql("ALTER TABLE Items ADD CONSTRAINT Chk_price CHECK(price >= 0)");
            
            CreateTable(
                "dbo.Order_Items",
                c => new
                    {
                        order_id = c.Long(nullable: false),
                        item_id = c.Long(nullable: false),
                        name = c.String(nullable: false),
                        item_price = c.Decimal(nullable: false, precision: 11, scale: 2),
                        quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.order_id, t.item_id })
                .ForeignKey("dbo.Items", t => t.item_id, cascadeDelete: false)
                .ForeignKey("dbo.Orders", t => t.order_id, cascadeDelete: false)
                .Index(t => t.order_id)
                .Index(t => t.item_id);

            Sql("ALTER TABLE Order_Items ADD CONSTRAINT Chk_quantity CHECK(quantity > 0)");
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        order_id = c.Long(nullable: false, identity: true),
                        instructions = c.String(),
                        status = c.String(defaultValue: "Placed"),
                        address_line1 = c.String(nullable: false),
                        address_line2 = c.String(),
                        city = c.String(nullable: false),
                        state = c.String(nullable: false),
                        pincode = c.String(nullable: false, maxLength: 6),
                        country = c.String(nullable: false),
                        created_at = c.DateTime(nullable: false, defaultValueSql: "SYSUTCDATETIME()") ,
                        updated_at = c.DateTime(nullable: false, defaultValueSql: "SYSUTCDATETIME()") ,
                        user_id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.order_id)
                .ForeignKey("dbo.Users", t => t.user_id, cascadeDelete: false)
                .Index(t => t.user_id);

            Sql("ALTER TABLE Orders ADD CONSTRAINT Chk_status CHECK(status IN ('Placed', 'Accepted', 'Rejected', 'Dispatched', 'Delivered', 'Cancelled'))");
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        user_id = c.Long(nullable: false, identity: true),
                        email = c.String(nullable: false, maxLength: 255),
                        phone_number = c.String(nullable: false, maxLength: 50),
                        password = c.String(),
                        name = c.String(),
                        role = c.String(defaultValue: "customer"),
                        wallet_balance = c.Decimal(nullable: false, precision: 11, scale: 2, defaultValue: 1000m),
                        is_active = c.Boolean(nullable: false, defaultValue: true) ,
                        created_at = c.DateTime(nullable: false, defaultValueSql: "SYSUTCDATETIME()") ,
                        updated_at = c.DateTime(nullable: false, defaultValueSql: "SYSUTCDATETIME()") ,
                    })
                .PrimaryKey(t => t.user_id)
                .Index(t => t.email, unique: true, name: "IX_User_Email")
                .Index(t => t.phone_number, unique: true, name: "IX_User_PhoneNumber");

            Sql("ALTER TABLE Users ADD CONSTRAINT Chk_role CHECK(role IN ('customer', 'owner', 'admin'))");
            Sql("ALTER TABLE Users ADD CONSTRAINT Chk_balance CHECK(wallet_balance >= 0)");

            CreateTable(
                "dbo.Owner_Manages_Restaurants",
                c => new
                    {
                        restaurant_id = c.Long(nullable: false),
                        user_id = c.Long(nullable: false),
                        created_at = c.DateTime(nullable: false, defaultValueSql: "SYSUTCDATETIME()") ,
                    })
                .PrimaryKey(t => new { t.restaurant_id, t.user_id })
                .ForeignKey("dbo.Restaurants", t => t.restaurant_id, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.user_id, cascadeDelete: false)
                .Index(t => t.restaurant_id)
                .Index(t => t.user_id);
            
            CreateTable(
                "dbo.User_Address_Type",
                c => new
                    {
                        user_id = c.Long(nullable: false),
                        address_id = c.Long(nullable: false),
                        address_type = c.String(defaultValue:"home"),
                    })
                .PrimaryKey(t => new { t.user_id, t.address_id })
                .ForeignKey("dbo.Addresses", t => t.address_id, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.user_id, cascadeDelete: false)
                .Index(t => t.user_id)
                .Index(t => t.address_id);
            
        }
        
        public override void Down()
        {
            Sql("ALTER TABLE Items DROP CONSTRAINT Chk_price");
            Sql("ALTER TABLE Order_Items DROP CONSTRAINT Chk_quantity");
            Sql("ALTER TABLE Orders DROP CONSTRAINT Chk_status");
            Sql("ALTER TABLE Users DROP CONSTRAINT Chk_role");
            Sql("ALTER TABLE Users DROP CONSTRAINT Chk_balance");
            DropForeignKey("dbo.Menus", "restaurant_id", "dbo.Restaurants");
            DropForeignKey("dbo.Menu_Items", "menu_id", "dbo.Menus");
            DropForeignKey("dbo.User_Address_Type", "user_id", "dbo.Users");
            DropForeignKey("dbo.User_Address_Type", "address_id", "dbo.Addresses");
            DropForeignKey("dbo.Owner_Manages_Restaurants", "user_id", "dbo.Users");
            DropForeignKey("dbo.Owner_Manages_Restaurants", "restaurant_id", "dbo.Restaurants");
            DropForeignKey("dbo.Orders", "user_id", "dbo.Users");
            DropForeignKey("dbo.Order_Items", "order_id", "dbo.Orders");
            DropForeignKey("dbo.Order_Items", "item_id", "dbo.Items");
            DropForeignKey("dbo.Menu_Items", "item_id", "dbo.Items");
            DropForeignKey("dbo.Restaurants", "address_id", "dbo.Addresses");
            DropIndex("dbo.User_Address_Type", new[] { "address_id" });
            DropIndex("dbo.User_Address_Type", new[] { "user_id" });
            DropIndex("dbo.Owner_Manages_Restaurants", new[] { "user_id" });
            DropIndex("dbo.Owner_Manages_Restaurants", new[] { "restaurant_id" });
            DropIndex("dbo.Users", "IX_User_PhoneNumber");
            DropIndex("dbo.Users", "IX_User_Email");
            DropIndex("dbo.Orders", new[] { "user_id" });
            DropIndex("dbo.Order_Items", new[] { "item_id" });
            DropIndex("dbo.Order_Items", new[] { "order_id" });
            DropIndex("dbo.Menu_Items", new[] { "item_id" });
            DropIndex("dbo.Menu_Items", new[] { "menu_id" });
            DropIndex("dbo.Menus", new[] { "restaurant_id" });
            DropIndex("dbo.Restaurants", new[] { "address_id" });
            DropTable("dbo.User_Address_Type");
            DropTable("dbo.Owner_Manages_Restaurants");
            DropTable("dbo.Users");
            DropTable("dbo.Orders");
            DropTable("dbo.Order_Items");
            DropTable("dbo.Items");
            DropTable("dbo.Menu_Items");
            DropTable("dbo.Menus");
            DropTable("dbo.Restaurants");
            DropTable("dbo.Addresses");
        }
    }
}
