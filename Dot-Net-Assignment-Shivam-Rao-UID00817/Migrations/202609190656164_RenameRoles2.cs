namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameRoles2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "role", c => c.Int(nullable: false, defaultValue:1));
            DropColumn("dbo.Users", "roles2");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "roles2", c => c.Int(nullable: false, defaultValue: 1));
            DropColumn("dbo.Users", "role");
        }
    }
}
