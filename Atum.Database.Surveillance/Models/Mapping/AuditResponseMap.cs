using Atum.Domain.SurveyManagement;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Database.Surveillance.Models.Mapping
{
    public class AuditResponseMap
            : EntityTypeConfiguration<Response>
    {
        public AuditResponseMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            //// Properties
            //this.Property(t => t.Id)
            //    .IsRequired()
            //    .HasMaxLength(128);
            this.Property(t => t.ResponseChoiceId).IsRequired();


            // Table & Column Mappings
            this.ToTable("AuditResponses");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ResponseChoiceId).HasColumnName("ResponseChoiceId");

            //// Relationships
            //this.HasOptional(t => t.User)
            //    .WithMany()
            //    .HasForeignKey(d => d.UserId);
        }
    }
}
