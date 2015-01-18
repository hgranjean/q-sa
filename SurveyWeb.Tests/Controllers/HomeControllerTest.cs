using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SurveyWeb.Controllers;
using SurveyWeb.App_Start;
using Microsoft.Practices.Unity;
using SurveyWeb.Repository;
using Microsoft.AspNet.Identity;
using SurveyWeb.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Atum.Repository.Surveillance;

namespace SurveyWeb.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        private IUnityContainer container;

        [AssemblyInitialize]
        public void Initialize()
        {
            var unityContainer = new UnityContainer();

            UnityConfig.RegisterTypes(unityContainer);
        }

        [TestMethod]
        public void Index()
        {
            // Arrange
            var taskRepository = container.Resolve<ITaskRepository>();
            var userManager = container.Resolve<UserManager<ApplicationUser>>();
            
            HomeController controller = new HomeController(taskRepository, userManager);

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.AreEqual("Modify this template to jump-start your ASP.NET MVC application.", result.ViewBag.Message);
        }

        [TestMethod]
        public void About()
        {
            // Arrange
            var taskRepository = container.Resolve<ITaskRepository>();
            var userManager = container.Resolve<UserManager<ApplicationUser>>();

            HomeController controller = new HomeController(taskRepository, userManager);

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Contact()
        {
            // Arrange
            var taskRepository = container.Resolve<ITaskRepository>();
            var userManager = container.Resolve<UserManager<ApplicationUser>>();

            HomeController controller = new HomeController(taskRepository, userManager);

            // Act
            ViewResult result = controller.Contact() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
