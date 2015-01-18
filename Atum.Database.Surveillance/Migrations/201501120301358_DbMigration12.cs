namespace Atum.Database.Surveillance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DbMigration12 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Events", "SurveyId", "dbo.Surveys");
            DropForeignKey("dbo.EventUsers", "EventId", "dbo.Events");
            DropForeignKey("dbo.EventUsers", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Questions", "BasisReference_ID", "dbo.TOCElements");
            DropForeignKey("dbo.FollowUps", "Audit_Id", "dbo.Audits");
            //DropForeignKey("dbo.Responses1", "Audit_Id", "dbo.Audits");
            DropIndex("dbo.Persons", new[] { "Address_ID" });
            DropIndex("dbo.Persons", new[] { "Department_ID" });
            DropIndex("dbo.Responses1", new[] { "Audit_ID" });
            DropIndex("dbo.Events", new[] { "SurveyId" });
            DropIndex("dbo.EventUsers", new[] { "EventId" });
            DropIndex("dbo.EventUsers", new[] { "UserId" });
            DropIndex("dbo.Questions", new[] { "BasisReference_ID" });
            DropIndex("dbo.ResponseChoices", new[] { "Question_ID" });
            DropPrimaryKey("dbo.Audits");
            CreateTable(
                "dbo.FollowUps",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Status = c.String(),
                        InitialDueDate = c.DateTime(nullable: false),
                        TimeSent = c.Int(nullable: false),
                        LastSent = c.DateTime(nullable: false),
                        AuditId = c.String(),
                        Category = c.String(),
                        ItemInspected = c.String(),
                        Score = c.String(),
                        EstimatedCompletionDate = c.DateTime(nullable: false),
                        ItemDetails = c.String(),
                        Area_Id = c.Int(),
                        InspectedBy_Id = c.String(maxLength: 128),
                        Observation_Id = c.Int(nullable: false),
                        Question_Id = c.Long(),
                        ResponsibleParty_Id = c.String(maxLength: 128),
                        Audit_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Areas", t => t.Area_Id)
                .ForeignKey("dbo.Persons", t => t.InspectedBy_Id)
                //.ForeignKey("dbo.Responses1", t => t.Observation_Id)
                .ForeignKey("dbo.Questions", t => t.Question_Id)
                .ForeignKey("dbo.Persons", t => t.ResponsibleParty_Id)
                .ForeignKey("dbo.Audits", t => t.Audit_Id)
                .Index(t => t.Area_Id)
                .Index(t => t.InspectedBy_Id)
                .Index(t => t.Observation_Id)
                .Index(t => t.Question_Id)
                .Index(t => t.ResponsibleParty_Id)
                .Index(t => t.Audit_Id);
            
            CreateTable(
                "dbo.Areas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Surveys1",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Guid = c.Guid(nullable: false),
                        Title = c.String(),
                        SurveyType = c.Int(nullable: false),
                        FirstQuestion_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.FirstQuestion_Id)
                .Index(t => t.FirstQuestion_Id);
            
            CreateTable(
                "dbo.StandardDocuments",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(),
                        OwnerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Chapters",
                c => new
                    {
                        ChapterId = c.Guid(nullable: false),
                        Title = c.String(),
                        Key = c.String(),
                        Level = c.Int(nullable: false),
                        ShortContent = c.String(),
                    })
                .PrimaryKey(t => t.ChapterId);
            
            CreateTable(
                "dbo.Standards",
                c => new
                    {
                        StandardId = c.Guid(nullable: false),
                        Title = c.String(),
                        Key = c.String(),
                        Level = c.Int(nullable: false),
                        ShortContent = c.String(),
                    })
                .PrimaryKey(t => t.StandardId);
            
            CreateTable(
                "dbo.PerformanceItems",
                c => new
                    {
                        PerformanceItemId = c.Guid(nullable: false),
                        Text = c.String(),
                        EPId = c.Int(nullable: false),
                        Title = c.String(),
                        Key = c.String(),
                        Level = c.Int(nullable: false),
                        ShortContent = c.String(),
                    })
                .PrimaryKey(t => t.PerformanceItemId);
            
            CreateTable(
                "dbo.ItemNotes",
                c => new
                    {
                        ItemNodeId = c.Guid(nullable: false),
                        Text = c.String(),
                        PerformanceItem_PerformanceItemId = c.Guid(),
                    })
                .PrimaryKey(t => t.ItemNodeId)
                .ForeignKey("dbo.PerformanceItems", t => t.PerformanceItem_PerformanceItemId)
                .Index(t => t.PerformanceItem_PerformanceItemId);
            
            AddColumn("dbo.Audits", "SurveyId", c => c.Int(nullable: false));
            AddColumn("dbo.Audits", "Surveillance_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Audits", "Surveyor_Id", c => c.String(maxLength: 128));
            //AddColumn("dbo.Responses1", "FollowUpId", c => c.Guid());
            //AddColumn("dbo.Responses1", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            //AddColumn("dbo.Responses1", "Person_Id", c => c.String(maxLength: 128));
            //AddColumn("dbo.Responses1", "Answer_Id", c => c.Long());
            //AddColumn("dbo.Responses1", "Question_Id", c => c.Long());
            AddColumn("dbo.Events", "OwnerId", c => c.String(maxLength: 128));
            AddColumn("dbo.Events", "Frequency", c => c.Int());
            AddColumn("dbo.Events", "CreatedDate", c => c.DateTime());
            AddColumn("dbo.Events", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Events", "Area_Id", c => c.Int());
            AddColumn("dbo.Events", "AssignedTo_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Events", "CreatedBy_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Events", "Template_Id", c => c.Long());
            AddColumn("dbo.Events", "FollowUp_Id", c => c.Guid());
            AddColumn("dbo.Questions", "PerformanceItemId", c => c.String());
            AddColumn("dbo.Questions", "Survey_Id", c => c.Long());
            AlterColumn("dbo.Audits", "Id", c => c.Int(nullable: false, identity: true));
            //AlterColumn("dbo.Responses1", "Audit_Id", c => c.Int());
            AddPrimaryKey("dbo.Audits", "Id");
            CreateIndex("dbo.Persons", "Address_Id");
            CreateIndex("dbo.Persons", "Department_Id");
            CreateIndex("dbo.Audits", "Surveillance_Id");
            CreateIndex("dbo.Audits", "Surveyor_Id");
            CreateIndex("dbo.Events", "OwnerId");
            CreateIndex("dbo.Events", "Area_Id");
            CreateIndex("dbo.Events", "AssignedTo_Id");
            CreateIndex("dbo.Events", "CreatedBy_Id");
            CreateIndex("dbo.Events", "Template_Id");
            CreateIndex("dbo.Events", "FollowUp_Id");
            CreateIndex("dbo.Questions", "Survey_Id");
            CreateIndex("dbo.ResponseChoices", "Question_Id");
            //CreateIndex("dbo.Responses1", "Person_Id");
            //CreateIndex("dbo.Responses1", "Answer_Id");
            //CreateIndex("dbo.Responses1", "Question_Id");
            //CreateIndex("dbo.Responses1", "Audit_Id");
            AddForeignKey("dbo.Events", "OwnerId", "dbo.Persons", "Id");
            AddForeignKey("dbo.Events", "Area_Id", "dbo.Areas", "Id");
            AddForeignKey("dbo.Events", "AssignedTo_Id", "dbo.Persons", "Id");
            AddForeignKey("dbo.Events", "CreatedBy_Id", "dbo.Persons", "Id");
            AddForeignKey("dbo.Questions", "Survey_Id", "dbo.Surveys1", "Id");
            AddForeignKey("dbo.Events", "Template_Id", "dbo.Surveys1", "Id");
            AddForeignKey("dbo.Events", "FollowUp_Id", "dbo.FollowUps", "Id");
            //AddForeignKey("dbo.Responses1", "Person_Id", "dbo.Persons", "Id");
            //AddForeignKey("dbo.Responses1", "Answer_Id", "dbo.ResponseChoices", "Id");
            //AddForeignKey("dbo.Responses1", "Question_Id", "dbo.Questions", "Id");
            AddForeignKey("dbo.Audits", "Surveillance_Id", "dbo.Events", "Id");
            AddForeignKey("dbo.Audits", "Surveyor_Id", "dbo.Persons", "Id");
            //AddForeignKey("dbo.Responses1", "Audit_Id", "dbo.Audits", "Id");
            DropColumn("dbo.Audits", "SurveyTitle");
            DropColumn("dbo.Events", "SurveyId");
            DropColumn("dbo.Events", "EventTypeId");
            DropColumn("dbo.Questions", "ReferenceElementId");
            DropColumn("dbo.Questions", "TOCReference");
            DropColumn("dbo.Questions", "BasisReference_ID");
            DropTable("dbo.EventUsers");
            //DropTable("dbo.TOCElements");
        }
        
        public override void Down()
        {
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
                "dbo.EventUsers",
                c => new
                    {
                        EventId = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.EventId, t.UserId });
            
            AddColumn("dbo.Questions", "BasisReference_ID", c => c.Long());
            AddColumn("dbo.Questions", "TOCReference", c => c.String());
            AddColumn("dbo.Questions", "ReferenceElementId", c => c.String());
            AddColumn("dbo.Events", "EventTypeId", c => c.Int(nullable: false));
            AddColumn("dbo.Events", "SurveyId", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Audits", "SurveyTitle", c => c.String());
            //DropForeignKey("dbo.Responses1", "Audit_Id", "dbo.Audits");
            DropForeignKey("dbo.ItemNotes", "PerformanceItem_PerformanceItemId", "dbo.PerformanceItems");
            DropForeignKey("dbo.Audits", "Surveyor_Id", "dbo.Persons");
            DropForeignKey("dbo.Audits", "Surveillance_Id", "dbo.Events");
            //DropForeignKey("dbo.Responses1", "Question_Id", "dbo.Questions");
            //DropForeignKey("dbo.Responses1", "Answer_Id", "dbo.ResponseChoices");
            DropForeignKey("dbo.FollowUps", "Audit_Id", "dbo.Audits");
            DropForeignKey("dbo.FollowUps", "ResponsibleParty_Id", "dbo.Persons");
            DropForeignKey("dbo.FollowUps", "Question_Id", "dbo.Questions");
            //DropForeignKey("dbo.FollowUps", "Observation_Id", "dbo.Responses1");
            //DropForeignKey("dbo.Responses1", "Person_Id", "dbo.Persons");
            DropForeignKey("dbo.FollowUps", "InspectedBy_Id", "dbo.Persons");
            DropForeignKey("dbo.Events", "FollowUp_Id", "dbo.FollowUps");
            DropForeignKey("dbo.Events", "Template_Id", "dbo.Surveys1");
            DropForeignKey("dbo.Questions", "Survey_Id", "dbo.Surveys1");
            DropForeignKey("dbo.Surveys1", "FirstQuestion_Id", "dbo.Questions");
            DropForeignKey("dbo.Events", "CreatedBy_Id", "dbo.Persons");
            DropForeignKey("dbo.Events", "AssignedTo_Id", "dbo.Persons");
            DropForeignKey("dbo.Events", "Area_Id", "dbo.Areas");
            DropForeignKey("dbo.Events", "OwnerId", "dbo.Persons");
            DropForeignKey("dbo.FollowUps", "Area_Id", "dbo.Areas");
            DropIndex("dbo.ItemNotes", new[] { "PerformanceItem_PerformanceItemId" });
            DropIndex("dbo.Responses1", new[] { "Audit_Id" });
            DropIndex("dbo.Responses1", new[] { "Question_Id" });
            DropIndex("dbo.Responses1", new[] { "Answer_Id" });
            DropIndex("dbo.Responses1", new[] { "Person_Id" });
            DropIndex("dbo.ResponseChoices", new[] { "Question_Id" });
            DropIndex("dbo.Questions", new[] { "Survey_Id" });
            DropIndex("dbo.Surveys1", new[] { "FirstQuestion_Id" });
            DropIndex("dbo.Events", new[] { "FollowUp_Id" });
            DropIndex("dbo.Events", new[] { "Template_Id" });
            DropIndex("dbo.Events", new[] { "CreatedBy_Id" });
            DropIndex("dbo.Events", new[] { "AssignedTo_Id" });
            DropIndex("dbo.Events", new[] { "Area_Id" });
            DropIndex("dbo.Events", new[] { "OwnerId" });
            DropIndex("dbo.FollowUps", new[] { "Audit_Id" });
            DropIndex("dbo.FollowUps", new[] { "ResponsibleParty_Id" });
            DropIndex("dbo.FollowUps", new[] { "Question_Id" });
            DropIndex("dbo.FollowUps", new[] { "Observation_Id" });
            DropIndex("dbo.FollowUps", new[] { "InspectedBy_Id" });
            DropIndex("dbo.FollowUps", new[] { "Area_Id" });
            DropIndex("dbo.Audits", new[] { "Surveyor_Id" });
            DropIndex("dbo.Audits", new[] { "Surveillance_Id" });
            DropIndex("dbo.Persons", new[] { "Department_Id" });
            DropIndex("dbo.Persons", new[] { "Address_Id" });
            DropPrimaryKey("dbo.Audits");
            AlterColumn("dbo.Responses1", "Audit_Id", c => c.Long());
            AlterColumn("dbo.Audits", "Id", c => c.Long(nullable: false, identity: true));
            DropColumn("dbo.Questions", "Survey_Id");
            DropColumn("dbo.Questions", "PerformanceItemId");
            DropColumn("dbo.Events", "FollowUp_Id");
            DropColumn("dbo.Events", "Template_Id");
            DropColumn("dbo.Events", "CreatedBy_Id");
            DropColumn("dbo.Events", "AssignedTo_Id");
            DropColumn("dbo.Events", "Area_Id");
            DropColumn("dbo.Events", "Discriminator");
            DropColumn("dbo.Events", "CreatedDate");
            DropColumn("dbo.Events", "Frequency");
            DropColumn("dbo.Events", "OwnerId");
            DropColumn("dbo.Responses1", "Question_Id");
            DropColumn("dbo.Responses1", "Answer_Id");
            DropColumn("dbo.Responses1", "Person_Id");
            DropColumn("dbo.Responses1", "Discriminator");
            DropColumn("dbo.Responses1", "FollowUpId");
            DropColumn("dbo.Audits", "Surveyor_Id");
            DropColumn("dbo.Audits", "Surveillance_Id");
            DropColumn("dbo.Audits", "SurveyId");
            DropTable("dbo.ItemNotes");
            DropTable("dbo.PerformanceItems");
            DropTable("dbo.Standards");
            DropTable("dbo.Chapters");
            DropTable("dbo.StandardDocuments");
            DropTable("dbo.Surveys1");
            DropTable("dbo.Areas");
            DropTable("dbo.FollowUps");
            AddPrimaryKey("dbo.Audits", "Id");
            CreateIndex("dbo.ResponseChoices", "Question_ID");
            CreateIndex("dbo.Questions", "BasisReference_ID");
            CreateIndex("dbo.EventUsers", "UserId");
            CreateIndex("dbo.EventUsers", "EventId");
            CreateIndex("dbo.Events", "SurveyId");
            CreateIndex("dbo.Responses1", "Audit_ID");
            CreateIndex("dbo.Persons", "Department_ID");
            CreateIndex("dbo.Persons", "Address_ID");
            AddForeignKey("dbo.Responses1", "Audit_Id", "dbo.Audits", "Id");
            AddForeignKey("dbo.FollowUps", "Audit_Id", "dbo.Audits", "Id");
            AddForeignKey("dbo.Questions", "BasisReference_ID", "dbo.TOCElements", "ID");
            AddForeignKey("dbo.EventUsers", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.EventUsers", "EventId", "dbo.Events", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Events", "SurveyId", "dbo.Surveys", "Id", cascadeDelete: true);
        }
    }
}
