using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Atum.Database.Surveillance.Models.Mapping;
using Atum.Domain.Basis.Domain.Schedule;
using Atum.Domain.Business;
using Atum.Domain.Common;
using Atum.Domain.Security.Domain;

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
        public DbSet<SurveyEvent> SurveyEvents { get; set; }
        public DbSet<Person> Persons { get; set; }

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
            modelBuilder.Configurations.Add(new SurveyEventMap());
        }
    }
}
