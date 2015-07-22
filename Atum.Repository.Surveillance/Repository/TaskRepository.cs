using Atum.Database.Surveillance.Models;
using Atum.Domain.Security.Domain;
using Atum.Domain.SurveyManagement;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Atum.Repository.Surveillance
{
    public interface ITaskRepository
    {
        IEnumerable<Event> GetPastDueTasks(string userId);

        //IEnumerable<Event> GetNextTasks(string userId);

        IEnumerable<Event> GetTasksForUser(string userId, DateTime fromDate, DateTime toDate);

        IEnumerable<Event> GetFacilityTasks(string userId);

        Event GetTask(string id);

        IEnumerable<Atum.Domain.Security.Domain.AspNetUser> GetUsersForTask(string taskId);

        void UpdateTask(Atum.Domain.SurveyManagement.Event evt);

        void UpdateUsersForTask(string taskId, IEnumerable<string> currentUsers, IEnumerable<string> selectedUsers);

        Event CreateTask(Event evt);
    }

    public class TaskRepository : ITaskRepository
    {
        private readonly AtumSurveillanceContext _context;
        private readonly string _connectionString;

        public TaskRepository(string connectionString)
        {
            this._connectionString = connectionString;
            
            this._context = new AtumSurveillanceContext();
        }

        public IEnumerable<Event> GetPastDueTasks(string userId)
        {
            throw new NotImplementedException();
            //return _context.EventUsers.Include(m => m.Event.Template).Where(m => m.UserId == userId && m.Event.Start < DateTime.Now);
        }


        public IEnumerable<Event> GetNextTasks(string userId)
        {
            throw new NotImplementedException();
            //return _context.EventUsers.Include(m => m.Event.Template).Where(m => m.UserId == userId && m.Event.Start >= DateTime.Now);            
        }

        public IEnumerable<Event> GetFacilityTasks(string userId)
        {
            // Gets list of hospitals for the userId and returns list of users for the given hospitals
            var query =
                from a in _context.UserHospitals.Where(m => m.UserId == userId)
                join b in _context.UserHospitals on a.HospitalId equals b.HospitalId
                select b;
            
            // Gets the events for the distinct user ids
            //return
            //    from a in query.Select(m => m.UserId).Distinct().AsQueryable()
            //    join b in _context.EventUsers.Include(m => m.Event.Template).Include(m => m.User).Include(m => m.User.Person) on a equals b.UserId
            //    select b;
            throw new NotImplementedException();

        }


        public IEnumerable<Event> GetTasksForUser(string userId, DateTime fromDate, DateTime toDate)
        {
            return _context.Events.AsEnumerable();
        }

        public Event GetTask(string id)
        {
            return _context.Events.FirstOrDefault(m => m.Id== id);
        }

        public IEnumerable<AspNetUser> GetUsersForTask(string taskId)
        {
            return from a in _context.Events
                   join b in _context.AspNetUsers on a.Id equals b.Id
                   where a.Id == taskId
                   select b;
        }


        public void UpdateTask(Event evt)
        {
            _context.Events.Attach(evt);
            _context.Entry(evt).CurrentValues.SetValues(evt);
            _context.Entry(evt).State = EntityState.Modified;
            _context.SaveChanges();
        }


        //TODO: Find References and Fix
        public void UpdateUsersForTask(string taskId, IEnumerable<string> currentUsers, IEnumerable<string> selectedUsers)
        {
            // Remove unselected items

            var availableUsers = _context.AspNetUsers;
            foreach (var item in availableUsers)
            {
                if (selectedUsers.Count(userId => userId == item.Id) == 0)
                {
                    //var toDelete = _context.EventUsers.FirstOrDefault(eventUser => eventUser.EventId == taskId && eventUser.UserId == item.Id);

                    //if (toDelete != default(EventUser))
                    //{
                    //    _context.EventUsers.Remove(toDelete);
                    //}
                }
            }
                        
            // Add newly selected items
            foreach (var userId in selectedUsers)
            {
                if (currentUsers.Count(user => user == userId) == 0)
                {
                    //_context.EventUsers.Add(new EventUser { EventId = taskId, UserId = userId });
                }
            }

            _context.SaveChanges();            
        }


        public Event CreateTask(Event evt)
        {
            var @event = _context.Events.Add(evt);
            _context.SaveChanges();
            return @event;
        }
    }
}