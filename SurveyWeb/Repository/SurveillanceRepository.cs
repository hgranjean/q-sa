using Atum.Database.Surveillance.Models;
using Atum.Domain.Common;
using Atum.Domain.SurveyManagement;
using Atum.Domain.Security.Domain;
using Atum.Domain.Business;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using SurveyWeb.Models;
using Microsoft.AspNet.Identity;
using Atum.Domain.Healthcare;
using Atum.Domain.QualityManagement.Auditing;

namespace SurveyWeb.Repository
{
    public interface ISurveillanceRepository
    {
        IEnumerable<string> GetResponses(string userId);

        IEnumerable<Person> GetPersons();
        
        ResponseEntry AddResponse(string userId);        

        Hospital GetHospital(string hospitalId);

        void AddPerson(Person person);

        void UpdatePerson(Person person);
        

        void SetPasswordHashAsync(ApplicationUser user, string password, UserManager<ApplicationUser> userManager);

        Hospital AddHospital(Hospital hospital);

        void UpdateHospital(Hospital hospital);

        IEnumerable<Area> GetAreas(string hospitalId);

        IEnumerable<Facility> GetFacilities();

        IEnumerable<Building> GetBuildings();

        IEnumerable<Department> GetDepartments();

        IEnumerable<Hospital> GetHospitals();

        void DeleteHospital(string hospitalId);
        
        void DeleteResponse(string responseId);

        void AddAudit(Audit audit);

        IEnumerable<Audit> GetAudits();

        void DeleteAudit(string auditId);
    }

    public class SurveillanceRepository : ISurveillanceRepository
    {
        private readonly AtumSurveillanceContext _context;

        public SurveillanceRepository(string connectionString)
        {
            _context = new AtumSurveillanceContext();
        }

        public IEnumerable<string> GetResponses(string userId)
        {
            return _context.Responses.Where(m => m.UserId == userId).Select(m => m.Id);
        }

        public IEnumerable<Person> GetPersons()
        {
            return _context.Persons;
        }
        
        public AspNetUser GetUser(string userId)
        {
            return _context.AspNetUsers.First(m => m.Id == userId);
        }

        public ResponseEntry AddResponse(string userId)
        {
            var user = _context.AspNetUsers.FirstOrDefault(m => m.Id == userId);

            var responseEntry = new ResponseEntry { Id = Guid.NewGuid().ToString("d"), User = user };            

            _context.Responses.Add(responseEntry);

            _context.SaveChanges();

            return responseEntry;
        }

        public IEnumerable<AspNetUser> GetUsers()
        {
            return _context.AspNetUsers;
        }


        public Hospital GetHospital(string hospitalId)
        {
            return _context.Hospitals.First(m => m.Id == hospitalId);
        }


        public void AddPerson(Person person)
        {
            _context.Persons.Add(person);
            // _context.Entry(person.Hospital).CurrentValues.SetValues(person.Hospital);
            if (_context.Entry(person.Hospital).State != EntityState.Added)
            {
                _context.Entry(person.Hospital).State = EntityState.Modified;
            }
            _context.SaveChanges();
        }


        public void UpdatePerson(Person person)
        {
            _context.Persons.Attach(person);
            _context.Entry(person).CurrentValues.SetValues(person);
            _context.Entry(person).State = EntityState.Modified;
            _context.SaveChanges();
        }
        

        public void SetPasswordHashAsync(ApplicationUser user, string password, UserManager<ApplicationUser> userManager)
        {
            using (var store = new UserStore<ApplicationUser>())
            {
                store.SetPasswordHashAsync(user, userManager.PasswordHasher.HashPassword(password)).ContinueWith(t =>
                    userManager.UpdateAsync(user)).Wait();

                _context.SaveChanges();
            }            
        }

        public IEnumerable<Hospital> GetHospitals()
        {
            return _context.Hospitals;
        }

        public Hospital AddHospital(Hospital hospital)
        {
            _context.Hospitals.Add(hospital);
            _context.SaveChanges();
            return _context.Hospitals.FirstOrDefault(m => m.Id == hospital.Id);
        }

        public void UpdateHospital(Hospital hospital)
        {
            _context.Hospitals.Attach(hospital);
            _context.Entry(hospital).CurrentValues.SetValues(hospital);
            _context.Entry(hospital).State = EntityState.Modified;
            _context.SaveChanges();
        }


        public IEnumerable<Area> GetAreas(string hospitalId)
        {
            // TODO: Load from the database
            yield return new Area("Area1", 1);
            yield return new Area("Area2", 2);
        }

        public IEnumerable<Facility> GetFacilities()
        {
            // TODO: Load from the database, i.e.
            // return _dbContext.Hospitals.Select(hospital => new Facility(hospital.Name, Int32.Parse(hospital.Id)));
            yield return new Facility("Facility1", 1);
            yield return new Facility("Facility2", 2);            
        }


        public IEnumerable<Building> GetBuildings()
        {
            // TODO: Load from the database
            yield return new Building("Building1", 1);
            yield return new Building("Building2", 2);
        }


        public IEnumerable<Department> GetDepartments()
        {
            // TODO: Load from the database
            yield return new Department("Department1", 1);
            yield return new Department("Department2", 2);
        }


        public void DeleteHospital(string hospitalId)
        {
            var toDelete = _context.Hospitals.FirstOrDefault(m => m.Id == hospitalId);
                        
            _context.Hospitals.Remove(toDelete);

            _context.SaveChanges();            
        }

        public void DeleteResponse(string responseId)
        {
            var toDelete = _context.Responses.FirstOrDefault(m => m.Id == responseId);

            _context.Responses.Remove(toDelete);

            _context.SaveChanges();            
        }

        //Audit Persistence
        public IEnumerable<Audit> GetAudits()
        {
            return _context.Audits;
        }

        public Audit AddAudit(Audit Audit)
        {
            _context.Audits.Add(Audit);
            _context.SaveChanges();
            return _context.Audits.FirstOrDefault(m => m.Id == Audit.Id);
        }

        public void UpdateAudit(Audit Audit)
        {
            _context.Audits.Attach(Audit);
            _context.Entry(Audit).CurrentValues.SetValues(Audit);
            _context.Entry(Audit).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteAudit(string   auditId)
        {
            var toDelete = _context.Audits.FirstOrDefault(m => m.Id == int.Parse(auditId));

            _context.Audits.Remove(toDelete);

            _context.SaveChanges();
        }
        
        void ISurveillanceRepository.AddAudit(Audit audit)
        {
            throw new NotImplementedException();
        }

    }
}