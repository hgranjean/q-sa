using Atum.Domain.SurveyManagement;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Database.Surveillance.Models.Mapping
{
    public class QuestionGroupMap : EntityTypeConfiguration<QuestionGroup>
    {
        public QuestionGroupMap()
        {
            // Primary Key
            this.HasKey(t => t.Number);

            this.Property(t => t.Title)
            .IsRequired();


            // Table & Column Mappings
            this.ToTable("QuestionGroups");

            //// Relationships
            //  this.HasRequired(q => q.SurveyTitle);
            //this.HasRequired(q => q.Survey).HasForeignKey(t => t.SurveyTitle);


        }


    }
}
