namespace Atum.Database.Surveillance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DbMigration16 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ItemNotes", "PerformanceItem_PerformanceItemId", "dbo.PerformanceItems");
            DropForeignKey("dbo.Standards", "ChapterId", "dbo.Chapters");
            DropForeignKey("dbo.PerformanceItems", "StandardId", "dbo.Standards");
            DropForeignKey("dbo.ItemNotes", "PerformanceItemId", "dbo.PerformanceItems");
            DropIndex("dbo.ItemNotes", new[] { "PerformanceItem_PerformanceItemId" });
            DropColumn("dbo.ItemNotes", "PerformanceItemId");
            RenameColumn(table: "dbo.ItemNotes", name: "PerformanceItem_PerformanceItemId", newName: "PerformanceItemId");
            DropPrimaryKey("dbo.Chapters");
            DropPrimaryKey("dbo.Standards");
            DropPrimaryKey("dbo.PerformanceItems");
            DropPrimaryKey("dbo.ItemNotes");
            AlterColumn("dbo.Chapters", "ChapterId", c => c.Guid(nullable: false, identity: true, defaultValueSql: "newsequentialid()"));
            AlterColumn("dbo.Standards", "StandardId", c => c.Guid(nullable: false, identity: true, defaultValueSql: "newsequentialid()"));
            AlterColumn("dbo.PerformanceItems", "PerformanceItemId", c => c.Guid(nullable: false, identity: true, defaultValueSql: "newsequentialid()"));
            AlterColumn("dbo.ItemNotes", "ItemNodeId", c => c.Guid(nullable: false, identity: true, defaultValueSql: "newsequentialid()"));
            AlterColumn("dbo.ItemNotes", "PerformanceItemId", c => c.Guid(nullable: false));
            AlterColumn("dbo.ItemNotes", "PerformanceItemId", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.Chapters", "ChapterId");
            AddPrimaryKey("dbo.Standards", "StandardId");
            AddPrimaryKey("dbo.PerformanceItems", "PerformanceItemId");
            AddPrimaryKey("dbo.ItemNotes", "ItemNodeId");
            CreateIndex("dbo.ItemNotes", "PerformanceItemId");
            AddForeignKey("dbo.ItemNotes", "PerformanceItemId", "dbo.PerformanceItems", "PerformanceItemId", cascadeDelete: true);
            AddForeignKey("dbo.Standards", "ChapterId", "dbo.Chapters", "ChapterId", cascadeDelete: true);
            AddForeignKey("dbo.PerformanceItems", "StandardId", "dbo.Standards", "StandardId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PerformanceItems", "StandardId", "dbo.Standards");
            DropForeignKey("dbo.Standards", "ChapterId", "dbo.Chapters");
            DropForeignKey("dbo.ItemNotes", "PerformanceItemId", "dbo.PerformanceItems");
            DropIndex("dbo.ItemNotes", new[] { "PerformanceItemId" });
            DropPrimaryKey("dbo.ItemNotes");
            DropPrimaryKey("dbo.PerformanceItems");
            DropPrimaryKey("dbo.Standards");
            DropPrimaryKey("dbo.Chapters");
            AlterColumn("dbo.ItemNotes", "PerformanceItemId", c => c.Guid());
            AlterColumn("dbo.ItemNotes", "PerformanceItemId", c => c.Int(nullable: false));
            AlterColumn("dbo.ItemNotes", "ItemNodeId", c => c.Guid(nullable: false));
            AlterColumn("dbo.PerformanceItems", "PerformanceItemId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Standards", "StandardId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Chapters", "ChapterId", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.ItemNotes", "ItemNodeId");
            AddPrimaryKey("dbo.PerformanceItems", "PerformanceItemId");
            AddPrimaryKey("dbo.Standards", "StandardId");
            AddPrimaryKey("dbo.Chapters", "ChapterId");
            RenameColumn(table: "dbo.ItemNotes", name: "PerformanceItemId", newName: "PerformanceItem_PerformanceItemId");
            AddColumn("dbo.ItemNotes", "PerformanceItemId", c => c.Int(nullable: false));
            CreateIndex("dbo.ItemNotes", "PerformanceItem_PerformanceItemId");
            AddForeignKey("dbo.ItemNotes", "PerformanceItemId", "dbo.PerformanceItems", "PerformanceItemId", cascadeDelete: true);
            AddForeignKey("dbo.PerformanceItems", "StandardId", "dbo.Standards", "StandardId", cascadeDelete: true);
            AddForeignKey("dbo.Standards", "ChapterId", "dbo.Chapters", "ChapterId", cascadeDelete: true);
            AddForeignKey("dbo.ItemNotes", "PerformanceItem_PerformanceItemId", "dbo.PerformanceItems", "PerformanceItemId");
        }
    }
}
