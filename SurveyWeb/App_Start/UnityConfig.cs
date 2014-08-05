using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using SurveyWeb.Repository;
using SurveyWeb.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Configuration;
using System.Data.Entity;

namespace SurveyWeb.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your types here
            // container.RegisterType<IProductRepository, ProductRepository>();
            // container.RegisterType<UserManager<ApplicationUser>>( new InjectionFactory( c => new UserManager<ApplicationUser>(new UserStore<ApplicationUser>( new ApplicationDbContext())), new HttpContextLifetimeManager() ));

            // Read more about DI with Unity at http://msdn.microsoft.com/en-us/library/dn178463(v=pandp.30).aspx

            string connectionString = ConfigurationManager.ConnectionStrings["AtumSurveillanceContext"].ConnectionString;

            // TODO: Register your types here                       
            container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(new HierarchicalLifetimeManager());
            container.RegisterType<UserManager<ApplicationUser>, UserManager<ApplicationUser>>(new HttpContextLifetimeManager());
            container.RegisterType<DbContext, ApplicationDbContext>(new HierarchicalLifetimeManager());

            container.RegisterType<ITaskRepository, TaskRepository>(new InjectionConstructor(connectionString));            
            container.RegisterType<ISurveillanceRepository, SurveillanceRepository>(new InjectionConstructor(connectionString));
            container.RegisterType<ISurveyRepository, SurveyRepository>(new InjectionConstructor(connectionString));            
        }
    }
}
