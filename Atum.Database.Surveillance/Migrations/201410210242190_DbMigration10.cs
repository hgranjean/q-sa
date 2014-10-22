namespace Atum.Database.Surveillance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DbMigration10 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "EventTypeId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "EventTypeId");
        }
    }
}
