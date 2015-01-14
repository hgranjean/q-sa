using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace SurveyWeb.Controllers
{
    public static class ModelHelper
    {
        public static void AddErrors(this Controller accountController, IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                accountController.ModelState.AddModelError("", error);
            }
        }

        public static void AddErrors(this Controller accountController, DbEntityValidationException e)
        {
            foreach (var eve in e.EntityValidationErrors)
            {
                Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);

                foreach (var ve in eve.ValidationErrors)
                {
                    Debug.WriteLine("- CreatedBy: \"{0}\", Error: \"{1}\"",
                                    ve.PropertyName, ve.ErrorMessage);

                    accountController.ModelState.AddModelError("", ve.ErrorMessage);
                }
            }
        }
    }
}