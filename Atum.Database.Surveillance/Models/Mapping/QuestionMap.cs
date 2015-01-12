using Atum.Domain.SurveyManagement;
using System.Data.Entity.ModelConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Database.Surveillance.Models.Mapping
{
    public class QuestionMap : EntityTypeConfiguration<Question>
    {
        public QuestionMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            
            // Table & Column Mappings
            this.ToTable("Questions");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Text).HasColumnName("Text");
            this.Property(t => t.Number).HasColumnName("Number");
            this.Property(t => t.Label).HasColumnName("Label");
            //this.CreatedBy(t => t.SurveyTitle).HasColumnName("SurveyTitle");
            this.Property(t => t.GroupNumber).HasColumnName("GroupNumber");

            //Relationship
            this.HasRequired(t => t.QuestionGroup).WithMany(q => q.Questions).HasForeignKey(t => t.GroupNumber);
            //this.HasRequired(t => t.Template).HasForeignKey(t => t.SurveyTitle);

        }
    }
}
