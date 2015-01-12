using Atum.Domain.SurveyManagement;
using System.Data.Entity.ModelConfiguration;

namespace Atum.Database.Surveillance.Models.Mapping
{
    public class ResponseChoiceMap
          : EntityTypeConfiguration<ResponseChoice>
    {

        public ResponseChoiceMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            //// Properties
            //this.CreatedBy(t => t.Id)
            //    .IsRequired()
            //    .HasMaxLength(128);

            this.Property(t => t.Key);
            this.Property(t => t.Number);
            this.Property(t => t.Text);


            // Table & Column Mappings
            this.ToTable("ResponseChoices");

        }
    }
}
