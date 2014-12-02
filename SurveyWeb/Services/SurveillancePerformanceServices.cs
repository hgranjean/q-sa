using Atum.Domain.Common;
using SurveyWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyWeb.Services
{
    public class SurveillancePerformanceServices
    {
        internal static IEnumerable<TaskViewModel> GetSurveyorSurveillances(int SurveyId)
        {
            yield return new TaskViewModel { Id = "1", Area = "Surveillance Area A", Start = DateTime.Today.AddDays(-5), End = DateTime.Today.AddDays(14), Owner = (new Person("John", "Q", "Surveyor")), CurrentState = "In-Progress", IssuesCount = "3" };
            yield return new TaskViewModel { Id = "2", Area = "Surveillance Area B", Start = DateTime.Today.AddMonths(1), End = DateTime.Today.AddMonths(1).AddDays(7), Owner = (new Person("John", "Q", "Surveyor")), CurrentState = "Pending", IssuesCount = "" };
        }

        internal static IEnumerable<ObservationViewModel> GetSurveyorObservations(int SurveyId)
        {
            yield return new ObservationViewModel { Id = "2", Summary = "Observations 1 Summary", FollowUpId = "1" };
            yield return new ObservationViewModel { Id = "3", Summary = "Observations 2 Summary", FollowUpId = "4" };
        }
    }
}