using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using MoviesApp.Data;
using MoviesApp.Models;
using MoviesApp.Models.ViewModels;
using MoviesApp.Repos.Interfaces;

namespace MoviesApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardRepos _dashboardRepos;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPhotoService _photoService;

        public DashboardController(IDashboardRepos dashboardRepos,
                                    IHttpContextAccessor httpContextAccessor,
                                    IPhotoService photoService)
        {
            _dashboardRepos = dashboardRepos;
            _httpContextAccessor = httpContextAccessor;
            _photoService = photoService;
        }

        private void MapUserEdit(AppUser user, UserVM editVM, ImageUploadResult photoResult)
        {
            user.Id = editVM.Id;
            user.ImageUrl = photoResult.Url.ToString();
        }

        public async Task<IActionResult> Index()
        {
            var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var user = await _dashboardRepos.GetUserById(curUserId);

            var userPlaylists = await _dashboardRepos.GetAllUserPlaylists();
            
            var dashboardVM = new UserVM()
            {
                //Playlists = userPlaylists,
                Id = curUserId,
                UserName = user.UserName,
                City = user.City,
                State = user.State,
                ImageUrl = user.ImageUrl
            };

            return View(dashboardVM);
        }

        public async Task<IActionResult> EditUserProfile()
        {
            var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var user = await _dashboardRepos.GetUserById(curUserId);

            if (user == null) return View("Error");

            var editUserVM = new UserVM()
            {
                Id = curUserId,
                UserName = user.UserName,
                City = user.City,
                State = user.State,
                ImageUrl = user.ImageUrl
            };

            return View(editUserVM);
        }

        [HttpPost]
        public async Task<IActionResult> editUserProfile(UserVM editVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to Edit Profile");
                return View("EditUserProfile", editVM);
            }

            var user = await _dashboardRepos.GetByIdNoTracking(editVM.Id);
            if (user.ImageUrl == "" || user.ImageUrl == null)
            {
                var photoResult = await _photoService.AddPhotoAsync(editVM.Image);
                MapUserEdit(user, editVM, photoResult);

                // Optmistic Concurrency - 
                _dashboardRepos.Update(user);
                return RedirectToAction("index");
            }
            else
            {
                try
                {
                    await _photoService.DeletePhotoAsync(user.ImageUrl);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(editVM);
                }

                var photoResult = await _photoService.AddPhotoAsync(editVM.Image);
                MapUserEdit(user, editVM, photoResult);
                
                // Optmistic Concurrency - 
                _dashboardRepos.Update(user);
                return RedirectToAction("Index");
            }
        }
    }
}
