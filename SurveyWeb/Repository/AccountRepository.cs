using Atum.Database.Surveillance.Models;
using Atum.Domain.Security.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyWeb.Repository
{
    public interface IAccountRepository
    {
        void AddUserHospital(string userId, string hospitalId);
        IEnumerable<UserHospital> GetHospitalUsers(string hospitalId);

        void AddRoleToUser(string roleName, string userId);

        AspNetRole GetRole(string roleName);
        AspNetUser GetUserWithRoles(string userId);

        IEnumerable<UserHospital> GetUserHospitals(string userId);

        AspNetUser GetUser(string userId);
        
        IEnumerable<AspNetUser> GetUsers();

        void DeleteUser(string userId);

        IEnumerable<AspNetRole> GetRoles();

        IEnumerable<UserHospital> GetUserHospitals();
    }
    public class AccountRepository : IAccountRepository
    {
        private readonly AtumSurveillanceContext _context;

        public AccountRepository(string connectionString)
        {
            _context = new AtumSurveillanceContext();
        }

        public void AddUserHospital(string userId, string hospitalId)
        {
            // Add user to hospital, if its not present there already
            if (_context.UserHospitals.Count(m => m.HospitalId == hospitalId && m.UserId == userId) == 0)
            {
                _context.UserHospitals.Add(new UserHospital { HospitalId = hospitalId, UserId = userId });
            }

            _context.SaveChanges();
        }

        public IEnumerable<UserHospital> GetHospitalUsers(string hospitalId)
        {
            return _context.UserHospitals.Where(m => m.HospitalId == hospitalId);
        }


        public void AddRoleToUser(string roleName, string userId)
        {
            var user = _context.AspNetUsers.FirstOrDefault(m => m.Id == userId);
            var managerRole = _context.AspNetRoles.FirstOrDefault(m => m.Name == roleName);

            if (user.AspNetRoles.Count(m => m.Id == managerRole.Id) == 0)
            {
                user.AspNetRoles.Add(managerRole);
            }

            _context.SaveChanges();
        }


        public AspNetRole GetRole(string roleName)
        {
            return _context.AspNetRoles.FirstOrDefault(m => m.Name == roleName);
        }

        public AspNetUser GetUserWithRoles(string userId)
        {
            return _context.AspNetUsers.Include("AspNetRoles").FirstOrDefault(m => m.Id == userId);            
        }


        public IEnumerable<UserHospital> GetUserHospitals(string userId)
        {
            return _context.UserHospitals.Where(m => m.UserId == userId);
        }

        public AspNetUser GetUser(string userId)
        {
            return _context.AspNetUsers.FirstOrDefault(m => m.Id == userId);
        }

        public IEnumerable<AspNetUser> GetUsers()
        {
            return _context.AspNetUsers;
        }

        public void DeleteUser(string userId)
        {
            var toDelete = _context.AspNetUsers.FirstOrDefault(m => m.Id == userId);

            _context.AspNetUsers.Remove(toDelete);

            _context.SaveChanges();
        }

        public IEnumerable<AspNetRole> GetRoles()
        {
            return _context.AspNetRoles;
        }

        public IEnumerable<UserHospital> GetUserHospitals()
        {
            return _context.UserHospitals;
        }
    }
}