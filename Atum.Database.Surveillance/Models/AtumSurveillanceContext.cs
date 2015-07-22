using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Atum.Database.Surveillance.Models.Mapping;
using Atum.Domain;
using Atum.Domain.Business;
using Atum.Domain.Common;
using Atum.Domain.Security.Domain;
using Atum.Domain.SurveyManagement;
using Atum.Domain.QualityManagement.Auditing;
using Atum.Domain.QualityManagement.Healthcare.Performance;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atum.Database.Surveillance.Models
{
    public partial class AtumSurveillanceContext : DbContext
    {
        static AtumSurveillanceContext()
        {
            System.Data.Entity.Database.SetInitializer<AtumSurveillanceContext>(null);
        }

        public AtumSurveillanceContext()
            : base("Name=AtumSurveillanceContext")
        {
        }
        
        public DbSet<AspNetRole> AspNetRoles { get; set; }
        public DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public DbSet<AspNetUser> AspNetUsers { get; set; }

        public DbSet<Hospital> Hospitals { get; set; }
        public DbSet<UserHospital> UserHospitals { get; set; }
        public DbSet<Event> Events { get; set; }
        
        public DbSet<SurveyEntry> Surveys { get; set; }
        public DbSet<ResponseEntry> Responses { get; set; }
        public DbSet<Audit> Audits { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionGroup> QuestionGroups { get; set; }
        public DbSet<ResponseChoice> ResponseChoices { get; set; }
        //public DbSet<EventUser> EventUsers { get; set; }
        public DbSet<Person> Persons { get; set; }
        // public DbSet<SurveyEvent> SurveyEvents { get; set; }

        //Standards Model Mapping
        //public DbSet<DocumentElement> DocumentElements { get; set; }
        public DbSet<StandardDocument> StandardDocuments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new PersonMap());
            modelBuilder.Configurations.Add(new AspNetRoleMap());
            modelBuilder.Configurations.Add(new AspNetUserClaimMap());
            modelBuilder.Configurations.Add(new AspNetUserLoginMap());
            modelBuilder.Configurations.Add(new AspNetUserMap());
            
            modelBuilder.Configurations.Add(new HospitalMap());
            modelBuilder.Configurations.Add(new UserHospitalMap());
            modelBuilder.Configurations.Add(new EventMap());

            modelBuilder.Configurations.Add(new SurveyMap());
            //modelBuilder.Configurations.Add(new EventUserMap());
            modelBuilder.Configurations.Add(new ResponseMap());
            modelBuilder.Configurations.Add(new AuditMap());
            modelBuilder.Configurations.Add(new QuestionMap());
            modelBuilder.Configurations.Add(new QuestionGroupMap());
            modelBuilder.Configurations.Add(new ResponseChoiceMap());


            //, defaultValueSql: "newsequentialid()"
            //Standard Documents and Guidelines
            modelBuilder.Ignore<TableOfContents>();
            modelBuilder.Ignore<DocumentElement>();
            modelBuilder.Entity<StandardDocument>().ToTable("StandardDocuments").HasKey(t => t.Id).Property(t => t.Id)
                .IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Chapter>().ToTable("Chapters").HasKey(t => t.ChapterId).Property(t => t.ChapterId)
                .IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity); 
            modelBuilder.Entity<Standard>().ToTable("Standards").HasKey(t=> t.StandardId).Property(t => t.StandardId)
                .IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<PerformanceItem>().ToTable("PerformanceItems").HasKey(t => t.PerformanceItemId).Property(t => t.PerformanceItemId)
                .IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<ItemNote>().ToTable("ItemNotes").HasKey(t => t.ItemNodeId).Property(t => t.ItemNodeId)
                .IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity); //.Ignore(t=> t.Id);

            modelBuilder.Entity<FollowUp>().ToTable("FollowUps").HasKey(t => t.Id).HasRequired(p => p.Observation);

        
        
        }
    }
}
