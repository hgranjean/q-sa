using System;
using Atum.Domain.SurveyManagement;
using System.Data.Entity.ModelConfiguration;
using  Atum.Domain.QualityManagement;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Database.Surveillance.Models.Mapping
{
    public class AuditMap : EntityTypeConfiguration<Audit>
    {
        public AuditMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            // Table & Column Mappings
            this.ToTable("Audits");
            this.Property(t => t.Id).HasColumnName("Id");
            //this.Property(t => t.SubcriberId).HasColumnName("Subscriber_Id");
            this.Property(t => t.Score).HasColumnName("Score");
            this.Property(t => t.DateCompleted).HasColumnName("DateCompleted");
            this.Property(t => t.DateStarted).HasColumnName("DateStarted");
        
        }

    }
}
