using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
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
        private readonly IPlaylistRepos _playlistRepos;

        public HomeController(ILogger<HomeController> logger, IPlaylistRepos playlistRepos)
        {
            _logger = logger;
            _playlistRepos = playlistRepos;
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
                
                if( homeVM.State != null)
                {
                    //homeVM.Playlists = await _playlistRepos.GetPlaylistByCity(homeVM.City);
                    homeVM.Playlists = await _playlistRepos.GetAll();
                }
                else
                {
                    homeVM.Playlists = null;
                }
                return View(homeVM);
            }
            catch
            {
                homeVM.Playlists = null;
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
