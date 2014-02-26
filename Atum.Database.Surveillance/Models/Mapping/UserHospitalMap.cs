using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Atum.Database.Surveillance.Models.Mapping
{
    public class UserHospitalMap : EntityTypeConfiguration<UserHospital>
    {
        public UserHospitalMap()
        {
            // Primary key
            this.HasKey(t => new {t.UserId, t.HospitalId});

            // Properties
            this.Property(t => t.UserId)
                .IsRequired()
                .HasMaxLength(128);
            this.Property(t => t.HospitalId)
                .IsRequired()
                .HasMaxLength(128);

            // Table & Column Mappings
            this.ToTable("UserHospitals");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.HospitalId).HasColumnName("HospitalId");

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany()
                .HasForeignKey(d => d.UserId);

            this.HasRequired(t => t.Hospital)
                .WithMany()
                .HasForeignKey(d => d.HospitalId);
        }
    }
}
