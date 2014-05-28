namespace Atum.Database.Surveillance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DbMigration2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Title = c.String(nullable: false, maxLength: 128),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                        Url = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SurveyEvents",
                c => new
                    {
                        SurveyId = c.String(nullable: false, maxLength: 128),
                        EventId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.SurveyId, t.EventId })
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .ForeignKey("dbo.Surveys", t => t.SurveyId, cascadeDelete: true)
                .Index(t => t.EventId)
                .Index(t => t.SurveyId);
            
            CreateTable(
                "dbo.Surveys",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SurveyEvents", "SurveyId", "dbo.Surveys");
            DropForeignKey("dbo.SurveyEvents", "EventId", "dbo.Events");
            DropIndex("dbo.SurveyEvents", new[] { "SurveyId" });
            DropIndex("dbo.SurveyEvents", new[] { "EventId" });
            DropTable("dbo.Surveys");
            DropTable("dbo.SurveyEvents");
            DropTable("dbo.Events");
        }
    }
}
