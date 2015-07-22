namespace Atum.Database.Surveillance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DbMigration14 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ItemNotes", "PerformanceItemId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ItemNotes", "PerformanceItemId");
        }
    }
}
