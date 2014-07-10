using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Atum.Domain.SurveyManagement;

namespace Atum.Database.Surveillance.Models.Mapping
{
    public class SurveyMap : EntityTypeConfiguration<SurveyEntry>
    {
        public SurveyMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(128);
            
            this.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(128);
            
            // Table & Column Mappings
            this.ToTable("Surveys");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Title).HasColumnName("Title");
        }
    }
}
