using Atum.Domain.QualityManagement.Auditing;
using Atum.Domain.QualityManagement.Healthcare.Performance;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Atum.Repository.Surveillance
{
    public class ObservationRepository : RepositoryBase<Observation>, IRepository<Observation>
    {

        public ObservationRepository(DbContext context)
            : base(context)
        {
        }
        public Observation Add(Observation subject)
        {
            throw new NotImplementedException();
        }

        public Observation Update(Observation subject)
        {
            throw new NotImplementedException();
        }

        public Observation FindById(int Id)
        {
            throw new NotImplementedException();
        }

        public Observation FindByGuid(Guid guid)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Observation> FindMatching(System.Linq.Expressions.Expression<Func<Observation, bool>> criteria)
        {
            throw new NotImplementedException();
        }

        public void Delete(Observation subject)
        {
            throw new NotImplementedException();
        }
    }
}