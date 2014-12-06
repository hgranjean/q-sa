using Atum.Domain.SurveyManagement;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SurveyWeb.Repository
{
    public class SurveyRepository1 : RepositoryBase<Survey>, IRepository<Survey>
    {

        public SurveyRepository1(DbContext context)
            : base(context)
        {
        }

        public Survey Add(Survey subject)
        {
            throw new NotImplementedException();
        }

        public Survey Update(Survey subject)
        {
            throw new NotImplementedException();
        }

        public Survey FindById(int Id)
        {
            throw new NotImplementedException();
        }

        public Survey FindByGuid(Guid guid)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Survey> FindMatching(System.Linq.Expressions.Expression<Func<Survey, bool>> criteria)
        {
            throw new NotImplementedException();
        }

        public void Delete(Survey subject)
        {
            throw new NotImplementedException();
        }
    }
}