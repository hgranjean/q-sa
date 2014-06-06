using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Atum.Domain.QualityManagement;
using SurveyWeb;
using SurveyWeb.Models;

namespace SurveyWeb
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class SurveyWeb : System.Web.HttpApplication
    {
        private static InitializeSimpleMembershipAttribute.SimpleMembershipInitializer _initializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            // WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            // Custom binders
            // ModelBinders.Binders.Add(typeof(QuestionGroupsViewModel), new QuestionGroupsViewModelBinder());

            // Ensure ASP.NET Simple Membership is initialized only once per app start
            LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
        }
    }
}