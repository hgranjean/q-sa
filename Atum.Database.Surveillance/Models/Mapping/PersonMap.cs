using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Atum.Domain.Common;

namespace Atum.Database.Surveillance.Models.Mapping
{
    public class PersonMap : EntityTypeConfiguration<Person>
    {
        public PersonMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.LastName)
                .HasMaxLength(50);
            
            this.Property(t => t.FirstName)
                .HasMaxLength(50);
            
            this.Property(t => t.MiddleName)
                .HasMaxLength(50);
            
            this.Property(t => t.Suffix)
                .HasMaxLength(20);

            this.Property(t => t.Email)
                .HasMaxLength(254);
            
            this.Property(t => t.PhoneNumber)
                .HasMaxLength(50);
            
            this.Property(t => t.JobTitle)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Persons");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.LastName).HasColumnName("LastName");
            this.Property(t => t.FirstName).HasColumnName("FirstName");
            this.Property(t => t.MiddleName).HasColumnName("MiddleName");
            this.Property(t => t.Suffix).HasColumnName("Suffix");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.PhoneNumber).HasColumnName("PhoneNumber");
            this.Property(t => t.JobTitle).HasColumnName("JobTitle");
            this.Property(t => t.HospitalId).HasColumnName("Hospital_Id");
		    // public Address Address { get; set; }
            // public DateTime DateOfBirth { get; set;}
            // public Hospital Hospital { get; set; }

            // Relationships
            this.HasOptional(t => t.Hospital)
                .WithMany()
                .HasForeignKey(d => d.HospitalId);
        }
    }
}
