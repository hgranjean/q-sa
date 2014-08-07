using Atum.Database.Surveillance.Models;
using Atum.Domain;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using Atum.Domain.Security.Domain;
using Atum.Domain.SurveyManagement;

namespace SurveyWeb.Repository
{
    public interface ITaskRepository
    {
        IEnumerable<EventUser> GetPastDueTasks(string userId);

        IEnumerable<EventUser> GetNextTasks(string userId);

        IEnumerable<EventUser> GetTasksForUser(string userId, DateTime fromDate, DateTime toDate);

        EventUser GetTask(string id);

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
        public IEnumerable<EventUser> GetPastDueTasks(string userId)
        {
            return _context.EventUsers.Include(m => m.Event.Survey).Where(m => m.UserId == userId && m.Event.Start < DateTime.Now);
        }


        public IEnumerable<EventUser> GetNextTasks(string userId)
        {
            return _context.EventUsers.Include(m => m.Event.Survey).Where(m => m.UserId == userId && m.Event.Start >= DateTime.Now);            
        }


        public IEnumerable<EventUser> GetTasksForUser(string userId, DateTime fromDate, DateTime toDate)
        {
            return _context.EventUsers.AsEnumerable();
        }

        public EventUser GetTask(string id)
        {
            return _context.EventUsers.FirstOrDefault(m => m.EventId == id);
        }

        public IEnumerable<AspNetUser> GetUsersForTask(string taskId)
        {
            return from a in _context.EventUsers
                   join b in _context.AspNetUsers on a.UserId equals b.Id
                   where a.EventId == taskId
                   select b;
        }


        public void UpdateTask(Event evt)
        {
            _context.Events.Attach(evt);
            _context.Entry(evt).CurrentValues.SetValues(evt);
            _context.Entry(evt).State = EntityState.Modified;
            _context.SaveChanges();
        }


        public void UpdateUsersForTask(string taskId, IEnumerable<string> currentUsers, IEnumerable<string> selectedUsers)
        {
            // Remove unselected items

            var availableUsers = _context.AspNetUsers;
            foreach (var item in availableUsers)
            {
                if (selectedUsers.Count(userId => userId == item.Id) == 0)
                {
                    var toDelete = _context.EventUsers.FirstOrDefault(eventUser => eventUser.EventId == taskId && eventUser.UserId == item.Id);

                    if (toDelete != default(EventUser))
                    {
                        _context.EventUsers.Remove(toDelete);
                    }
                }
            }
                        
            // Add newly selected items
            foreach (var userId in selectedUsers)
            {
                if (currentUsers.Count(user => user == userId) == 0)
                {
                    _context.EventUsers.Add(new EventUser { EventId = taskId, UserId = userId });
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