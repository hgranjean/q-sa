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
using SurveyWeb.Models.QualityManager.SurveillanceManagement;
using SurveyWeb.Models.QualityManager.SurveillanceTracking;

namespace SurveyWeb.Services
{
    public class SurveillanceManagementServices
    {
        private readonly ISurveillanceRepository _repository;

        public SurveillanceManagementServices(ISurveillanceRepository repository)
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
               

        internal ResponseEntry AddResponse(string userId)
        {
            return _repository.AddResponse(userId);
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


        
        //Audits - Completed Surveys
        internal void AddAudit(Audit audit)
        {
            _repository.AddAudit(audit);

        }

        internal IEnumerable<Audit> GetAudits()
        {
            return _repository.GetAudits();
        }

        internal void DeleteAudit(string auditId)
        {
            _repository.DeleteAudit(auditId);
        }

        internal IEnumerable<SurveillanceScheduleAnnualViewModel> GetAreaAnnualSchedule()
        {
            yield return new SurveillanceScheduleAnnualViewModel { Area = "A", January = "X", February = "", March = "", April = "", May = "", June = "", July = "X", August = "", September = "", October = "", November = "", December = "" };
            yield return new SurveillanceScheduleAnnualViewModel { Area = "B", January = "X", February = "", March = "", April = "", May = "", June = "", July = "X", August = "", September = "", October = "", November = "", December = "" };
            yield return new SurveillanceScheduleAnnualViewModel { Area = "C", January = "", February = "X", March = "", April = "", May = "", June = "", July = "", August = "X", September = "", October = "", November = "", December = "" };
            yield return new SurveillanceScheduleAnnualViewModel { Area = "D", January = "", February = "", March = "X", April = "", May = "", June = "", July = "", August = "", September = "X", October = "", November = "", December = "" };
            yield return new SurveillanceScheduleAnnualViewModel { Area = "E", January = "", February = "", March = "", April = "X", May = "", June = "", July = "", August = "", September = "", October = "X", November = "", December = "" };

        }

        internal IEnumerable<SurveillanceCompletionViewModel> GetAreaSurveillanceCompletion()
        {
            for (int i = 0; i < 4; i++)
            {
                List<FieldData> data = new List<FieldData>();
                data.AddRange(getData(i));
                yield return new SurveillanceCompletionViewModel { Area = "A" + i, PercentComplete = i + "%",  Data= data};
            }

        }

        private IEnumerable<FieldData> getData(int Id)
        {
            for (int i = 0; i < 4; i++)
            {
                yield return new FieldData { Key = i.ToString(), ID = Id, Label="Element" + i,Value="X" + i};
                
            }
        }
    }   
}