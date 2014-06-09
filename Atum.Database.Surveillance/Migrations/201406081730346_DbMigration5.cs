namespace Atum.Database.Surveillance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DbMigration5 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Events", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.SurveyEvents", "EventId", "dbo.Events");
            DropForeignKey("dbo.SurveyEvents", "SurveyId", "dbo.Surveys");
            DropIndex("dbo.Events", new[] { "UserId" });
            DropIndex("dbo.SurveyEvents", new[] { "EventId" });
            DropIndex("dbo.SurveyEvents", new[] { "SurveyId" });
            CreateTable(
                "dbo.EventUsers",
                c => new
                    {
                        EventId = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.EventId, t.UserId })
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.EventId)
                .Index(t => t.UserId);
            
            AddColumn("dbo.Events", "SurveyId", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Surveys", "Title", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Events", "SurveyId");
            AddForeignKey("dbo.Events", "SurveyId", "dbo.Surveys", "Id", cascadeDelete: true);
            DropColumn("dbo.Events", "Url");
            DropColumn("dbo.Events", "UserId");
            DropTable("dbo.SurveyEvents");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SurveyEvents",
                c => new
                    {
                        SurveyId = c.String(nullable: false, maxLength: 128),
                        EventId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.SurveyId, t.EventId });
            
            AddColumn("dbo.Events", "UserId", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Events", "Url", c => c.String());
            DropForeignKey("dbo.EventUsers", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.EventUsers", "EventId", "dbo.Events");
            DropForeignKey("dbo.Events", "SurveyId", "dbo.Surveys");
            DropIndex("dbo.EventUsers", new[] { "UserId" });
            DropIndex("dbo.EventUsers", new[] { "EventId" });
            DropIndex("dbo.Events", new[] { "SurveyId" });
            DropColumn("dbo.Surveys", "Title");
            DropColumn("dbo.Events", "SurveyId");
            DropTable("dbo.EventUsers");
            CreateIndex("dbo.SurveyEvents", "SurveyId");
            CreateIndex("dbo.SurveyEvents", "EventId");
            CreateIndex("dbo.Events", "UserId");
            AddForeignKey("dbo.SurveyEvents", "SurveyId", "dbo.Surveys", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SurveyEvents", "EventId", "dbo.Events", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Events", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
