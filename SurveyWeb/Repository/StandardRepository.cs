using Atum.Domain.QualityManagement.Healthcare.Performance;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SurveyWeb.Repository
{
    public class StandardRepository : RepositoryBase<Standard>, IRepository<Standard>
    {

        public StandardRepository(DbContext context):base(context)
        {
        }
        public Standard Add(Standard subject)
        {
            throw new NotImplementedException();
        }

        public Standard Update(Standard subject)
        {
            throw new NotImplementedException();
        }

        public Standard FindById(int Id)
        {
            throw new NotImplementedException();
        }

        public Standard FindByGuid(Guid guid)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Standard> FindMatching(System.Linq.Expressions.Expression<Func<Standard, bool>> criteria)
        {
            throw new NotImplementedException();
        }

        public void Delete(Standard subject)
        {
            throw new NotImplementedException();
        }
    }
}