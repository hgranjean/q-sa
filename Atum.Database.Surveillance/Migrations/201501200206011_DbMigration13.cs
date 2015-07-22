namespace Atum.Database.Surveillance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DbMigration13 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Chapters", "StandardDocument_Id", c => c.Long());
            AddColumn("dbo.Standards", "Chapter_ChapterId", c => c.Guid());
            AddColumn("dbo.PerformanceItems", "Standard_StandardId", c => c.Guid());
            CreateIndex("dbo.Chapters", "StandardDocument_Id");
            CreateIndex("dbo.Standards", "Chapter_ChapterId");
            CreateIndex("dbo.PerformanceItems", "Standard_StandardId");
            AddForeignKey("dbo.PerformanceItems", "Standard_StandardId", "dbo.Standards", "StandardId");
            AddForeignKey("dbo.Standards", "Chapter_ChapterId", "dbo.Chapters", "ChapterId");
            AddForeignKey("dbo.Chapters", "StandardDocument_Id", "dbo.StandardDocuments", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Chapters", "StandardDocument_Id", "dbo.StandardDocuments");
            DropForeignKey("dbo.Standards", "Chapter_ChapterId", "dbo.Chapters");
            DropForeignKey("dbo.PerformanceItems", "Standard_StandardId", "dbo.Standards");
            DropIndex("dbo.PerformanceItems", new[] { "Standard_StandardId" });
            DropIndex("dbo.Standards", new[] { "Chapter_ChapterId" });
            DropIndex("dbo.Chapters", new[] { "StandardDocument_Id" });
            DropColumn("dbo.PerformanceItems", "Standard_StandardId");
            DropColumn("dbo.Standards", "Chapter_ChapterId");
            DropColumn("dbo.Chapters", "StandardDocument_Id");
        }
    }
}
