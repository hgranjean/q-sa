using System.Data.Entity.ModelConfiguration;

namespace Atum.Database.Surveillance.Models.Mapping
{
    public class SurveyEventMap : EntityTypeConfiguration<SurveyEvent>
    {
        public SurveyEventMap()
        {
            // Primary key
            this.HasKey(t => new { t.SurveyId, t.EventId });

            // Properties
            this.Property(t => t.SurveyId)
                .IsRequired();
            this.Property(t => t.EventId)
                .IsRequired()
                .HasMaxLength(128);

            // Table & Column Mappings
            this.ToTable("SurveyEvents");
            this.Property(t => t.SurveyId).HasColumnName("SurveyId");
            this.Property(t => t.EventId).HasColumnName("EventId");

            // Relationships
            this.HasRequired(t => t.Event)
                .WithMany()
                .HasForeignKey(d => d.EventId);

            this.HasRequired(t => t.Survey)
                .WithMany()
                .HasForeignKey(d => d.SurveyId);
        }
    }
}
