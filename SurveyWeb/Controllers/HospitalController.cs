using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Atum.Database.Surveillance.Models;
using SurveyWeb.Models;

namespace MvcApplication1.Controllers
{
    public class HospitalController : Controller
    {
        private AtumSurveillanceContext _dbContext = null;

        public HospitalController()
        {
            _dbContext = new AtumSurveillanceContext();
        }

        public IEnumerable<Hospital> GetHospitals()
        {
            return _dbContext.Hospitals;
        }

        public ActionResult Index()
        {
            var model = _dbContext.Hospitals;
            
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Save(Hospital model)
        {
            model.Id = Guid.NewGuid().ToString();

            _dbContext.Hospitals.Add(model);

            _dbContext.SaveChanges();
            
            return RedirectToAction("Index");
        }
        
        public ActionResult Edit(string id)
        {
            var model = _dbContext.Hospitals.First(item => item.Id == id);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Hospital model)
        {
            var original = _dbContext.Hospitals.Find(model.Id);

            _dbContext.Entry(original).CurrentValues.SetValues(model);
            
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Details(string id)
        {
            var model = _dbContext.Hospitals.First(item => item.Id == id);

            return View(model);
        }

        public ActionResult Delete(string id)
        {
            var model = _dbContext.Hospitals.First(item => item.Id == id);

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(Hospital model)
        {
            var toDelete = _dbContext.Hospitals.First(item => item.Id == model.Id);

            if (toDelete != null)
            {
                _dbContext.Hospitals.Remove(toDelete);

                _dbContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult UserHospitalIndex()
        {
            var availableHospitals = _dbContext.Hospitals.ToList();
            var model = new List<UserHospitalViewModel>();
            
            model.Add(new UserHospitalViewModel
                {
                    AvailableHospitals = availableHospitals,
                    Hospitals = new List<Hospital>(),
                    UserId = Guid.NewGuid().ToString()
                });
            
            return View("UserHospitalIndex", model);
        }
    }
}
