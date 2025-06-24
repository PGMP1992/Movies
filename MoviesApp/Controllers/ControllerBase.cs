using Microsoft.AspNetCore.Mvc;
using MoviesApp.Services;

namespace MoviesApp.Controllers
{
    public class ControllerBase : Controller
    {
        public void HandleApiException(WebApiException ex)
        {
            //if (ex.ErrorResponse != null && 
            //    ex.ErrorResponse.Errors != null && 
            //    ex.ErrorResponse.Errors.Any())
            //{
            //    foreach (var error in ex.ErrorResponse.Errors)
            //    {
            //        ModelState.AddModelError(error.Key, string.Join("; ", error.Value));
            //    }
            //}
            //else 
            if (ex.Response != null)
            {
                ModelState.AddModelError("Error", ex.Response.ErrorMessage);
            }
            else
            {
                ModelState.AddModelError("Error", ex.Message);
            }
        }
    }
}
