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
        private static readonly LearningServices LearningService;
        private static readonly StandardsManagementServices StandardsManagementService;


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
            else if (typeof(T) == typeof(LearningServices))
            {
                return LearningService as T;
            }
            else if (typeof(T) == typeof(StandardsManagementServices))
            {
                return StandardsManagementService as T;
            }
            return null;
        }
    }
}