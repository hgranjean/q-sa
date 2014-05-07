using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyWeb.Models
{
    public class ConfirmMessageBoxViewModel : ViewModelBase
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
    }
}