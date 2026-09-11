namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class emailValidationAndNotNull : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "status", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "password", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "name", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "role", c => c.String(nullable: false));
            AlterColumn("dbo.User_Address_Type", "address_type", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.User_Address_Type", "address_type", c => c.String());
            AlterColumn("dbo.Users", "role", c => c.String());
            AlterColumn("dbo.Users", "name", c => c.String());
            AlterColumn("dbo.Users", "password", c => c.String());
            AlterColumn("dbo.Orders", "status", c => c.String());
        }
    }
}
