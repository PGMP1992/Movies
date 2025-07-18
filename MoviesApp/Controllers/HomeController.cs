using CloudinaryDotNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Movies.DataAccess.Models;
using Movies.DataAccess.ViewModels;
using Movies.Models;
using MoviesApp.Services;
using MoviesApp.Services.Interfaces;
using System.Diagnostics;

namespace MoviesApp.Controllers
{
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        //private readonly IUserRepos _usersRepos;
        //private readonly IUserService _userService;
        private readonly IWebApiExecutor _webApiExecutor;

        public HomeController(ILogger<HomeController> logger, IWebApiExecutor webApiExecutor)
        {
            _logger = logger;
            _webApiExecutor = webApiExecutor;

        }

        public async Task<IActionResult> Index() // Using IPInfo to get locations IP 
        {
            //var ipInfo = new IPInfo();
            var homeVM = new HomeVM();
            try
            {
                //string url = "https://ipinfo.io?be339e669dc21b"; //IPInfo.IO My Private Token
                //using (var httpClient = new HttpClient())
                //{
                //    var response = await httpClient.GetStringAsync(url);
                //    ipInfo = JsonConvert.DeserializeObject<IPInfo>(response);
                //}

                //RegionInfo myRI1 = new RegionInfo(ipInfo.Country);
                //ipInfo.Country = myRI1.EnglishName;
                //homeVM.City = ipInfo.City;
                //homeVM.State = ipInfo.Region;

                //if (homeVM.State != null)
                //{
                //homeVM.Users = await _usersRepos.GetAll();
                homeVM.Users = await _webApiExecutor.InvokeGet<List<AppUserDto>>($"Users/GetAll");
                //}
                return View(homeVM);
            }
            catch (WebApiException ex)
            {
                HandleApiException(ex);
                TempData["error"] = "Api Exception " + ex.ErrorResponse.Title;
                homeVM.Users = null;
            }
            return View(homeVM);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
