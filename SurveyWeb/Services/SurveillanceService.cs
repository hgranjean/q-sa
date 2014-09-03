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
using Atum.Domain.Healthcare;

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

        internal void DeleteResponse(string responseId)
        {
            _repository.DeleteResponse(responseId);
        }

        internal IEnumerable<Person> GetPersons()
        {
            return _repository.GetPersons();
        }        
               

        internal ResponseEntry AddResponse(AspNetUser user)
        {
            return _repository.AddResponse(user);
        }        

        internal IEnumerable<Hospital> GetHospitals()
        {
            return _repository.GetHospitals();
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
               

        internal void SetPasswordHashAsync(ApplicationUser user, string password, UserManager<ApplicationUser> userManager)
        {
            _repository.SetPasswordHashAsync(user, password, userManager);
        }

        internal IEnumerable<Area> GetAreas(string hospitalId)
        {
            return _repository.GetAreas(hospitalId);
        }

        internal IEnumerable<Facility> GetFacilities()
        {
            return _repository.GetFacilities();
        }

        internal IEnumerable<Building> GetBuildings()
        {
            return _repository.GetBuildings();
        }

        internal IEnumerable<Department> GetDepartments()
        {
            return _repository.GetDepartments();
        }

        internal void DeleteHospital(string hospitalId)
        {
            _repository.DeleteHospital(hospitalId);
        }
    }   
}