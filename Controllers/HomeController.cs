using Microsoft.AspNetCore.Mvc;
using MoviesApp.Helpers;
using MoviesApp.Interfaces;
using MoviesApp.Models;
using MoviesApp.ViewModels;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Globalization;
using System.Net;

namespace MoviesApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUsersRepos _usersRepos;

        public HomeController(ILogger<HomeController> logger, IUsersRepos usersRepos)
        {
            _logger = logger;
            _usersRepos = usersRepos;
        }

        public async Task<IActionResult> Index() // Using IPInfo to get locations IP 
        {
            var ipInfo = new IPInfo();
            var homeVM = new HomeVM();
            try
            {
                string url = "https://ipinfo.io?be339e669dc21b"; //IPInfo.IO My Private Token
                var info = new WebClient().DownloadString(url);
                ipInfo = JsonConvert.DeserializeObject<IPInfo>(info);
                RegionInfo myRI1 = new RegionInfo(ipInfo.Country);
                ipInfo.Country = myRI1.EnglishName;
                homeVM.City = ipInfo.City;
                homeVM.State = ipInfo.Region;

                if (homeVM.State != null)
                {
                    //homeVM.UserName = await _usersRepos.;
                    homeVM.Users = await _usersRepos.GetAllUsers();
                }
                else
                {
                    homeVM.Users = null;
                }
                return View(homeVM);
            }
            catch
            {
                homeVM.Users = null;
            }
            return View(homeVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
