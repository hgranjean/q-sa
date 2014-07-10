namespace Atum.Database.Surveillance.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DbMigration3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Persons",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(),
                        LastName = c.String(maxLength: 50),
                        FirstName = c.String(maxLength: 50),
                        MiddleName = c.String(maxLength: 50),
                        Suffix = c.String(maxLength: 20),
                        Email = c.String(maxLength: 254),
                        PhoneNumber = c.String(maxLength: 50),
                        JobTitle = c.String(maxLength: 50),
                        DateOfBirth = c.DateTime(nullable: false),
                        Address_ID = c.Long(),
                        Department_ID = c.Long(),
                        Hospital_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.Address_ID)
                .ForeignKey("dbo.Departments", t => t.Department_ID)
                .ForeignKey("dbo.Hospitals", t => t.Hospital_Id)
                .Index(t => t.Address_ID)
                .Index(t => t.Department_ID)
                .Index(t => t.Hospital_Id);
            
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        Street1 = c.String(),
                        Street2 = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Zip = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.AspNetUsers", "PersonId", c => c.String(maxLength: 128));
            AddColumn("dbo.Hospitals", "DomainName", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Hospitals", "Industry", c => c.String());
            CreateIndex("dbo.AspNetUsers", "PersonId");
            AddForeignKey("dbo.AspNetUsers", "PersonId", "dbo.Persons", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "PersonId", "dbo.Persons");
            DropForeignKey("dbo.Persons", "Hospital_Id", "dbo.Hospitals");
            DropForeignKey("dbo.Persons", "Department_ID", "dbo.Departments");
            DropForeignKey("dbo.Persons", "Address_ID", "dbo.Addresses");
            DropIndex("dbo.AspNetUsers", new[] { "PersonId" });
            DropIndex("dbo.Persons", new[] { "Hospital_Id" });
            DropIndex("dbo.Persons", new[] { "Department_ID" });
            DropIndex("dbo.Persons", new[] { "Address_ID" });
            DropColumn("dbo.Hospitals", "Industry");
            DropColumn("dbo.Hospitals", "DomainName");
            DropColumn("dbo.AspNetUsers", "PersonId");
            DropTable("dbo.Departments");
            DropTable("dbo.Addresses");
            DropTable("dbo.Persons");
        }
    }
}
