namespace Atum.Database.Surveillance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DbMigration11 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Audits",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SurveyTitle = c.String(),
                        DateStarted = c.DateTime(nullable: false),
                        DateCompleted = c.DateTime(nullable: false),
                        SubcriberId = c.Int(nullable: false),
                        Score = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Responses1",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuestionId = c.Int(nullable: false),
                        ResponseChoiceId = c.Int(nullable: false),
                        AnswerKey = c.String(),
                        Text = c.String(),
                        Audit_ID = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Audits", t => t.Audit_ID)
                .Index(t => t.Audit_ID);
            
            CreateTable(
                "dbo.QuestionGroups",
                c => new
                    {
                        Number = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Number);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Text = c.String(),
                        Number = c.Int(nullable: false),
                        Rank = c.Int(nullable: false),
                        ReferenceElementId = c.String(),
                        GroupNumber = c.Int(nullable: false),
                        Label = c.String(),
                        QuestionType = c.Int(nullable: false),
                        TOCReference = c.String(),
                        BasisReference_ID = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TOCElements", t => t.BasisReference_ID)
                .ForeignKey("dbo.QuestionGroups", t => t.GroupNumber, cascadeDelete: true)
                .Index(t => t.GroupNumber)
                .Index(t => t.BasisReference_ID);
            
            CreateTable(
                "dbo.TOCElements",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        Title = c.String(),
                        Level = c.Int(nullable: false),
                        ShortContent = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ResponseChoices",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        Text = c.String(),
                        Value = c.String(),
                        Number = c.String(),
                        Key = c.String(),
                        Question_ID = c.Long(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Questions", t => t.Question_ID)
                .Index(t => t.Question_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ResponseChoices", "Question_ID", "dbo.Questions");
            DropForeignKey("dbo.Questions", "GroupNumber", "dbo.QuestionGroups");
            DropForeignKey("dbo.Questions", "BasisReference_ID", "dbo.TOCElements");
            DropForeignKey("dbo.Responses1", "Audit_ID", "dbo.Audits");
            DropIndex("dbo.ResponseChoices", new[] { "Question_ID" });
            DropIndex("dbo.Questions", new[] { "BasisReference_ID" });
            DropIndex("dbo.Questions", new[] { "GroupNumber" });
            DropIndex("dbo.Responses1", new[] { "Audit_ID" });
            DropTable("dbo.ResponseChoices");
            DropTable("dbo.TOCElements");
            DropTable("dbo.Questions");
            DropTable("dbo.QuestionGroups");
            DropTable("dbo.Responses1");
            DropTable("dbo.Audits");
        }
    }
}
