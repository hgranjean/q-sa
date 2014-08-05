using Atum.Database.Surveillance.Models;
using Atum.Domain.Common;
using Atum.Domain.SurveyManagement;
using Atum.Domain.Security.Domain;
using System.Collections.Generic;
using System.Linq;
using System;

namespace SurveyWeb.Repository
{
    public interface ISurveillanceRepository
    {
        IEnumerable<string> GetResponses(string userId);

        IEnumerable<Atum.Domain.Common.Person> GetPersons();

        AspNetUser GetUser(string userId);

        ResponseEntry AddResponse(AspNetUser user);
        IEnumerable<AspNetUser> GetUsers();
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
    }
}