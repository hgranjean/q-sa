using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Atum.Domain.Business;

namespace Atum.Database.Surveillance.Models.Mapping
{
    public class HospitalMap : EntityTypeConfiguration<Hospital>
    {
        public HospitalMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(128);
            
            this.Property(t => t.DomainName)
                .IsRequired()
                .HasMaxLength(128);
            
            // Table & Column Mappings
            this.ToTable("Hospitals");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.DomainName).HasColumnName("DomainName");
        }
    }
}
