using Atum.Domain;
using Atum.Domain.Security.Domain;
using Atum.Domain.SurveyManagement;
using SurveyWeb.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyWeb.Services
{
    public class TaskService
    {
        private readonly ITaskRepository _repository;

        public TaskService(ITaskRepository repository)
        {
            this._repository = repository;
        }

        public IEnumerable<Event> GetPastDueTasks(string userId)
        {
            return _repository.GetPastDueTasks(userId);
        }


        public IEnumerable<Event> GetNextTasks(string userId)
        {
            return _repository.GetNextTasks(userId);
        }

        internal IEnumerable<Event> GetTasksForUser(string userId, DateTime fromDate, DateTime toDate)
        {
            return _repository.GetTasksForUser(userId, fromDate, toDate);
        }

        internal IEnumerable<Event> GetFacilityTasksForUser(string userId)
        {
            return _repository.GetFacilityTasks(userId);
        }

        internal Event GetTask(string id)
        {
            return _repository.GetTask(id);
        }

        internal IEnumerable<AspNetUser> GetUsersForTask(string taskId)
        {
            return _repository.GetUsersForTask(taskId);
        }

        internal void UpdateTask(Event evt)
        {
            _repository.UpdateTask(evt);
        }

        internal void UpdateUsersForTask(string taskId, IEnumerable<string> currentUsers, IEnumerable<string> selectedUsers)
        {
            _repository.UpdateUsersForTask(taskId, currentUsers, selectedUsers);
        }

        internal Event CreateTask(Event evt)
        {
            return _repository.CreateTask(evt);
        }
    }    
}