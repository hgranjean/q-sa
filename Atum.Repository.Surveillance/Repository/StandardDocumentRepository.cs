using Atum.Domain.QualityManagement.Healthcare.Performance;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Atum.Repository.Surveillance
{
    public class StandardDocumentRepository : RepositoryBase<StandardDocument>, IRepository<StandardDocument>
    {

        public StandardDocumentRepository(DbContext context):base(context)
        {
        }
        public StandardDocument Add(StandardDocument subject)
        {
            throw new NotImplementedException();
        }

        public StandardDocument Update(StandardDocument subject)
        {
            throw new NotImplementedException();
        }

        public StandardDocument FindById(int Id)
        {
            throw new NotImplementedException();
        }

        public StandardDocument FindByGuid(Guid guid)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<StandardDocument> FindMatching(System.Linq.Expressions.Expression<Func<StandardDocument, bool>> criteria)
        {
            throw new NotImplementedException();
        }

        public void Delete(StandardDocument subject)
        {
            throw new NotImplementedException();
        }
    }
}