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
            ViewBag.Users = _dbContext.AspNetUsers.ToList();
            
            var model = GetUserHospitals();

            return View("UserHospitalIndex", model);
        }

        private IEnumerable<UserHospitalViewModel> GetUserHospitals()
        {
            // TODO: Consier moving into ViewBag
            var availableHospitals = _dbContext.Hospitals.ToList();

            var userHospitalMap = new Dictionary<string, List<Hospital>>();
            foreach (var userHospital in _dbContext.UserHospitals)
            {
                if (!userHospitalMap.ContainsKey(userHospital.UserId))
                {
                    userHospitalMap.Add(userHospital.UserId, new List<Hospital>());
                }

                userHospitalMap[userHospital.UserId].Add(userHospital.Hospital);
            }
            
            foreach (var userId in userHospitalMap.Keys)
            {
                yield return new UserHospitalViewModel
                    {
                        AvailableHospitals = availableHospitals,
                        Hospitals = userHospitalMap[userId],
                        UserId = userId
                    };
            }
        }

        public ActionResult CreateUserHospital(string userId, string hospitalId)
        {
            var model = new UserHospital {UserId = userId, HospitalId = hospitalId};
            
            _dbContext.UserHospitals.Add(model);

            _dbContext.SaveChanges();

            return RedirectToAction("UserHospitalIndex", "Hospital");
        }

        public ActionResult EditUserHospital(string userId)
        {
            // TODO: Consier moving into ViewBag
            var availableHospitals = _dbContext.Hospitals.ToList();

            var model = GetUserHospitals().Where(item => item.UserId == userId);

            ViewBag.UserName = _dbContext.AspNetUsers.First(user => user.Id == userId).UserName;

            // If user has no user to hospital association records, create a viewmodel with empty lists
            if (!model.Any())
            {
                return View("UserHospitalEdit",
                                   new UserHospitalViewModel
                                       {
                                           AvailableHospitals = availableHospitals,
                                           Hospitals = new List<Hospital>(),
                                           UserId = userId
                                       });
            }
            
            // Otherwise return the viewmodel
            return View("UserHospitalEdit", model.First());
        }

        [HttpPost]
        public ActionResult EditUserHospital(UserHospitalViewModel viewModel)
        {
            var model = GetUserHospitals().Where(item => item.UserId == viewModel.UserId);

            var userHospitalViewModel = model.First();

            if (viewModel.SelectedHospitals != null)
            {
                // Remove unselected items
                foreach (var item in userHospitalViewModel.Hospitals)
                {
                    if (viewModel.SelectedHospitals.Count(hospitalId => hospitalId == item.Id) == 0)
                    {
                        var toDelete = _dbContext.UserHospitals.First(userHospital => userHospital.HospitalId == item.Id && userHospital.UserId == viewModel.UserId);

                        _dbContext.UserHospitals.Remove(toDelete);
                    }
                }

                // Add newly selected items
                foreach (var hospitalId in viewModel.SelectedHospitals)
                {
                    if (userHospitalViewModel.Hospitals.Count(hospital => hospital.Id == hospitalId) == 0)
                    {
                        _dbContext.UserHospitals.Add(new UserHospital { HospitalId = hospitalId, UserId = viewModel.UserId });
                    }
                }

                _dbContext.SaveChanges();
            }

            return EditUserHospital(viewModel.UserId);
        }
        
        public ActionResult DeleteUserHospital(string userId, string hospitalId)
        {
            var model = _dbContext.UserHospitals.Find(new[] {userId, hospitalId});

            _dbContext.UserHospitals.Remove(model);

            _dbContext.SaveChanges();

            return RedirectToAction("UserHospitalIndex", "Hospital");
        }
    }
}
