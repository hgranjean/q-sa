using SurveyWeb.Repository;
using Atum.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Atum.Domain.Security.Domain;
using Atum.Domain.SurveyManagement;
using Atum.Domain.Business;
using SurveyWeb.Models;
using Microsoft.AspNet.Identity;

namespace SurveyWeb.Services
{
    public class SurveillanceService
    {
        private readonly ISurveillanceRepository _repository;

        public SurveillanceService(ISurveillanceRepository repository)
        {
            _repository = repository;
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
        
        internal Hospital GetHospital(string hospitalId)
        {
            return _repository.GetHospital(hospitalId);
        }

        internal Hospital AddHospital(Hospital hospital)
        {
            return _repository.AddHospital(hospital);
        }

        internal void UpdateHospital(Hospital hospital)
        {
            _repository.UpdateHospital(hospital);
        }

        internal void AddPerson(Person person)
        {
            _repository.AddPerson(person);
        }

        internal void UpdatePerson(Person person)
        {
            _repository.UpdatePerson(person);
        }

        internal void DeleteUser(string userId)
        {
            _repository.DeleteUser(userId);            
        }

        internal void SetPasswordHashAsync(ApplicationUser user, string password, UserManager<ApplicationUser> userManager)
        {
            _repository.SetPasswordHashAsync(user, password, userManager);
        }
    }   
}