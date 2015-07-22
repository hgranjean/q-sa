namespace Atum.Database.Surveillance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DbMigration15 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Chapters", "StandardDocument_Id", "dbo.StandardDocuments");
            DropForeignKey("dbo.Standards", "Chapter_ChapterId", "dbo.Chapters");
            DropForeignKey("dbo.PerformanceItems", "Standard_StandardId", "dbo.Standards");
            DropIndex("dbo.Chapters", new[] { "StandardDocument_Id" });
            DropIndex("dbo.Standards", new[] { "Chapter_ChapterId" });
            DropIndex("dbo.PerformanceItems", new[] { "Standard_StandardId" });
            RenameColumn(table: "dbo.Chapters", name: "StandardDocument_Id", newName: "StandardDocumentId");
            RenameColumn(table: "dbo.Standards", name: "Chapter_ChapterId", newName: "ChapterId");
            RenameColumn(table: "dbo.PerformanceItems", name: "Standard_StandardId", newName: "StandardId");
            AlterColumn("dbo.Chapters", "StandardDocumentId", c => c.Long(nullable: false));
            AlterColumn("dbo.Standards", "ChapterId", c => c.Guid(nullable: false));
            AlterColumn("dbo.PerformanceItems", "StandardId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Chapters", "StandardDocumentId");
            CreateIndex("dbo.Standards", "ChapterId");
            CreateIndex("dbo.PerformanceItems", "StandardId");
            AddForeignKey("dbo.Chapters", "StandardDocumentId", "dbo.StandardDocuments", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Standards", "ChapterId", "dbo.Chapters", "ChapterId", cascadeDelete: true);
            AddForeignKey("dbo.PerformanceItems", "StandardId", "dbo.Standards", "StandardId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PerformanceItems", "StandardId", "dbo.Standards");
            DropForeignKey("dbo.Standards", "ChapterId", "dbo.Chapters");
            DropForeignKey("dbo.Chapters", "StandardDocumentId", "dbo.StandardDocuments");
            DropIndex("dbo.PerformanceItems", new[] { "StandardId" });
            DropIndex("dbo.Standards", new[] { "ChapterId" });
            DropIndex("dbo.Chapters", new[] { "StandardDocumentId" });
            AlterColumn("dbo.PerformanceItems", "StandardId", c => c.Guid());
            AlterColumn("dbo.Standards", "ChapterId", c => c.Guid());
            AlterColumn("dbo.Chapters", "StandardDocumentId", c => c.Long());
            RenameColumn(table: "dbo.PerformanceItems", name: "StandardId", newName: "Standard_StandardId");
            RenameColumn(table: "dbo.Standards", name: "ChapterId", newName: "Chapter_ChapterId");
            RenameColumn(table: "dbo.Chapters", name: "StandardDocumentId", newName: "StandardDocument_Id");
            CreateIndex("dbo.PerformanceItems", "Standard_StandardId");
            CreateIndex("dbo.Standards", "Chapter_ChapterId");
            CreateIndex("dbo.Chapters", "StandardDocument_Id");
            AddForeignKey("dbo.PerformanceItems", "Standard_StandardId", "dbo.Standards", "StandardId");
            AddForeignKey("dbo.Standards", "Chapter_ChapterId", "dbo.Chapters", "ChapterId");
            AddForeignKey("dbo.Chapters", "StandardDocument_Id", "dbo.StandardDocuments", "Id");
        }
    }
}
