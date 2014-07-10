namespace Atum.Database.Surveillance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DbMigration7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Responses", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Responses", "UserId");
            AddForeignKey("dbo.Responses", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Responses", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Responses", new[] { "UserId" });
            DropColumn("dbo.Responses", "UserId");
        }
    }
}
