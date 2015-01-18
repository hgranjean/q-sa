using Atum.Database.Surveillance.Models;
using Atum.Domain.Security.Domain;
using Atum.Repository.Surveillance;
using System.Collections.Generic;

namespace SurveyWeb.Services
{
    public class AccountService
    {
        private readonly IAccountRepository _repository;

        public AccountService(IAccountRepository repository)
        {
            _repository = repository;
        }
        
        internal AspNetUser GetUser(string userId)
        {
            return _repository.GetUser(userId);
        }

        internal IEnumerable<AspNetUser> GetUsers()
        {
            return _repository.GetUsers();
        }

        internal IEnumerable<AspNetUser> GetUsersWithProfiles()
        {
            return _repository.GetUsersWithProfiles();
        }

        internal void DeleteUser(string userId)
        {
            _repository.DeleteUser(userId);
        }
        internal void UpdateUser(AspNetUser user)
        {
            _repository.UpdateUser(user);
        }

        public void AddUserHospital(string userId, string hospitalId)
        {
            _repository.AddUserHospital(userId, hospitalId);
        }

        internal IEnumerable<UserHospital> GetHospitalUsers(string hospitalId)
        {
            return _repository.GetHospitalUsers(hospitalId);
        }

        internal IEnumerable<UserHospital> GetUserHospitals(string userId)
        {
            return _repository.GetUserHospitals(userId);
        }

        public void AddRoleToUser(string roleName, string userId)
        {
            _repository.AddRoleToUser(roleName, userId);            
        }

        internal AspNetRole GetRole(string roleName)
        {
            return _repository.GetRole(roleName);
        }

        internal AspNetUser GetUserWithRoles(string userId)
        {
            return _repository.GetUserWithRoles(userId);
        }

        internal IEnumerable<AspNetRole> GetRoles()
        {
            return _repository.GetRoles();
        }

        internal IEnumerable<UserHospital> GetUserHospitals()
        {
            return _repository.GetUserHospitals();
        }        
    }
}