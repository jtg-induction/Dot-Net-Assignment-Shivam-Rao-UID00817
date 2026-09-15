namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedRefreshTokenTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Refresh_Tokens",
                c => new
                    {
                        token_id = c.Guid(nullable: false, identity: true),
                        refresh_token = c.String(nullable: false),
                        user_id = c.Long(nullable: false),
                        created_at = c.DateTime(nullable: false),
                        expires_at = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.token_id)
                .ForeignKey("dbo.Users", t => t.user_id, cascadeDelete: false)
                .Index(t => t.user_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Refresh_Tokens", "user_id", "dbo.Users");
            DropIndex("dbo.Refresh_Tokens", new[] { "user_id" });
            DropTable("dbo.Refresh_Tokens");
        }
    }
}
