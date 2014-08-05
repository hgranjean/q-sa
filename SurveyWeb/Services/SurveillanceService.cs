using SurveyWeb.Repository;
using Atum.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Atum.Domain.Security.Domain;
using Atum.Domain.SurveyManagement;

namespace SurveyWeb.Services
{
    public class SurveillanceService
    {
        private readonly ISurveillanceRepository _repository;

        public SurveillanceService(ISurveillanceRepository repository)
        {
            this._repository = repository;
        }

        internal IEnumerable<string> GetResponses(string userId)
        {
            return _repository.GetResponses(userId);            
        }

        internal IEnumerable<Person> GetPersons()
        {
            return _repository.GetPersons();
        }

        internal AspNetUser GetUser(string userId)
        {
            return _repository.GetUser(userId);
        }

        internal ResponseEntry AddResponse(AspNetUser user)
        {
            return _repository.AddResponse(user);
        }

        internal IEnumerable<AspNetUser> GetUsers()
        {
            return _repository.GetUsers();
        }
    }   
}