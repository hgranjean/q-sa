using Atum.Database.Surveillance.Models;
using SurveyWeb.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyWeb.Services
{
    public class AccountService
    {
        private readonly IAccountRepository _repository;

        public AccountService(IAccountRepository repository)
        {
            _repository = repository;
        }

        public void AddUserHospital(string userId, string hospitalId)
        {
            _repository.AddUserHospital(userId, hospitalId);
        }

        internal IEnumerable<UserHospital> GetHospitalUsers(string hospitalId)
        {
            return _repository.GetHospitalUsers(hospitalId);
        }

        public void AddRoleToUser(string roleName, string userId)
        {
            _repository.AddRoleToUser(roleName, userId);            
        }
    }
}