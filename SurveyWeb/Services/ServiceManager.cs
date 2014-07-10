using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyWeb.Services
{
    internal class ServiceManager
    {
        private static readonly PersistenceServices persistenceServices;
        private static readonly MailService MailService;


        static ServiceManager()
        {
            persistenceServices = new PersistenceServices();
            MailService = new MailService();
        }

        public static T GetService<T>() where T : class
        {
            if (typeof(T) == typeof(PersistenceServices))
            {
                return persistenceServices as T;
            }
            else if (typeof (T) == typeof (MailService))
            {
                return MailService as T;
            }
            return null;
        }
    }
}