using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rules.Domain;

namespace Rules.WebEditor.Models.Actions
{
    public class SendMailActionViewModel
    {
        private SendMailAction Model { get; set; }

        public string Name
        {
            get { return Model.Name; }
            set { Model.Name = value; }
        }

        public SendMailActionViewModel(SendMailAction model)
        {
            this.Model = model;
        }

        public Expression From { get { return new Expression(Model.From); } set { Model.From = Expression.ToString(value); } }
        public Expression To { get { return new Expression(Model.To); } set { Model.To = Expression.ToString(value); } }
        public Expression Subject { get { return new Expression(Model.Subject); } set { Model.Subject = Expression.ToString(value); } }
        public Expression Body { get { return new Expression(Model.Body); } set { Model.Body = Expression.ToString(value); } }
    }
}