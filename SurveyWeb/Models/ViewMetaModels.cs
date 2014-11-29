using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyWeb.Models
{
    public class ViewMetaModels
    {
    }
    

    public class FieldData
    {
        public int ID { get; set; }
        public string Key { get; set; }
        public String Name { get; set; }
        public String Label { get; set; }
        public String Value { get; set; }
    }
}