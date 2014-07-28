using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Atum.Domain.Security.Domain;

namespace Atum.Database.Surveillance.Models.Mapping
{
    public class AspNetUserMap : EntityTypeConfiguration<AspNetUser>
    {
        public AspNetUserMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(128);

            /*this.Property(t => t.Discriminator)
                .IsRequired()
                .HasMaxLength(128);*/
            
                       

            // Table & Column Mappings
            this.ToTable("AspNetUsers");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.PasswordHash).HasColumnName("PasswordHash");
            this.Property(t => t.SecurityStamp).HasColumnName("SecurityStamp");
            /*this.Property(t => t.Discriminator).HasColumnName("Discriminator");*/
            this.Property(t => t.PersonId).HasColumnName("PersonId");

            this.Property(t => t.IsConfirmed).HasColumnName("IsConfirmed");

            this.Property(t => t.Email).HasColumnName("Email");

            this.Property(t => t.EmailConfirmed).HasColumnName("EmailConfirmed");

            this.Property(t => t.PhoneNumber).HasColumnName("PhoneNumber");
            
            // Relationships
            this.HasOptional(t => t.Person)
                .WithMany()
                .HasForeignKey(d => d.PersonId);
        }
    }
}
