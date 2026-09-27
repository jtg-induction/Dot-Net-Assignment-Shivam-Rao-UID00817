namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedMenusAndMenuItems : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Menu_Items", "item_id", "dbo.Items");
            DropForeignKey("dbo.Menu_Items", "menu_id", "dbo.Menus");
            DropForeignKey("dbo.Menus", "restaurant_id", "dbo.Restaurants");
            DropIndex("dbo.Menu_Items", new[] { "menu_id" });
            DropIndex("dbo.Menu_Items", new[] { "item_id" });
            DropIndex("dbo.Menus", new[] { "restaurant_id" });
            AddColumn("dbo.Items", "restaurant_id", c => c.Long(nullable: false));
            CreateIndex("dbo.Items", "restaurant_id");
            AddForeignKey("dbo.Items", "restaurant_id", "dbo.Restaurants", "restaurant_id", cascadeDelete: false);
            DropTable("dbo.Menu_Items");
            DropTable("dbo.Menus");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Menus",
                c => new
                    {
                        menu_id = c.Long(nullable: false, identity: true),
                        restaurant_id = c.Long(nullable: false),
                        name = c.String(nullable: false),
                        is_active = c.Boolean(nullable: false),
                        created_at = c.DateTime(nullable: false),
                        updated_at = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.menu_id);
            
            CreateTable(
                "dbo.Menu_Items",
                c => new
                    {
                        menu_id = c.Long(nullable: false),
                        item_id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.menu_id, t.item_id });
            
            DropForeignKey("dbo.Items", "restaurant_id", "dbo.Restaurants");
            DropIndex("dbo.Items", new[] { "restaurant_id" });
            DropColumn("dbo.Items", "restaurant_id");
            CreateIndex("dbo.Menus", "restaurant_id");
            CreateIndex("dbo.Menu_Items", "item_id");
            CreateIndex("dbo.Menu_Items", "menu_id");
            AddForeignKey("dbo.Menus", "restaurant_id", "dbo.Restaurants", "restaurant_id", cascadeDelete: true);
            AddForeignKey("dbo.Menu_Items", "menu_id", "dbo.Menus", "menu_id", cascadeDelete: true);
            AddForeignKey("dbo.Menu_Items", "item_id", "dbo.Items", "item_id", cascadeDelete: true);
        }
    }
}
