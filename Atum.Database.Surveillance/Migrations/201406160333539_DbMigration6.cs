namespace Atum.Database.Surveillance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DbMigration6 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Responses",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.Persons", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Persons", "UserId", c => c.String());
            DropTable("dbo.Responses");
        }
    }
}
