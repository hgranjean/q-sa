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

namespace SurveyWeb.Repository
{
    public interface ISurveillanceRepository
    {
        IEnumerable<string> GetResponses(string userId);

        IEnumerable<Atum.Domain.Common.Person> GetPersons();

        AspNetUser GetUser(string userId);

        ResponseEntry AddResponse(AspNetUser user);
        IEnumerable<AspNetUser> GetUsers();

        Hospital GetHospital(string hospitalId);

        void AddPerson(Person person);

        void UpdatePerson(Person person);

        void DeleteUser(string userId);

        void SetPasswordHashAsync(ApplicationUser user, string password, UserManager<ApplicationUser> userManager);

        Hospital AddHospital(Hospital hospital);

        void UpdateHospital(Hospital hospital);

        IEnumerable<Area> GetAreas(string hospitalId);

        IEnumerable<Facility> GetFacilities();

        IEnumerable<Building> GetBuildings();

        IEnumerable<Department> GetDepartments();
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

        public ResponseEntry AddResponse(AspNetUser user)
        {
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
        }


        public void UpdatePerson(Person person)
        {
            _context.Persons.Attach(person);
            _context.Entry(person).CurrentValues.SetValues(person);
            _context.Entry(person).State = EntityState.Modified;
        }


        public void DeleteUser(string userId)
        {
            var toDelete = _context.AspNetUsers.FirstOrDefault(m => m.Id == userId);

            _context.AspNetUsers.Remove(toDelete);

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


        public Hospital AddHospital(Hospital hospital)
        {
            _context.Hospitals.Add(hospital);

            _context.SaveChanges();

            return hospital;
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
    }
}