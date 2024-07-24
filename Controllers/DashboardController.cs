using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoviesApp.Data;
using MoviesApp.Models;
using MoviesApp.Repos.Interfaces;
using MoviesApp.ViewModels;

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

        [Authorize]
        private void MapUserEdit(AppUser user, EditUserDashboardVM editVM, ImageUploadResult photoResult)
        {
            user.Id = editVM.Id;
            user.ProfileImageryUrl = photoResult.Url.ToString();
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var user = await _dashboardRepos.GetUserById(curUserId);

            var userPlaylists = await _dashboardRepos.GetAllUserPlaylists();
            
            var dashboardVM = new DashboardVM()
            {
                Playlists = userPlaylists,
                Id = curUserId,
                UserName = user.UserName,
                City = user.City,
                State = user.State,
                ProfileImageUrl = user.ProfileImageryUrl
            };

            return View(dashboardVM);
        }

        [Authorize]
        public async Task<IActionResult> EditUserProfile()
        {
            var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var user = await _dashboardRepos.GetUserById(curUserId);

            if (user == null) return View("Error");

            var editUserVM = new EditUserDashboardVM()
            {
                Id = curUserId,
                UserName = user.UserName,
                City = user.City,
                State = user.State,
                ProfileImageUrl = user.ProfileImageryUrl
            };

            return View(editUserVM);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> editUserProfile(EditUserDashboardVM editVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to Edit Profile");
                return View("EditUserProfile", editVM);
            }

            var user = await _dashboardRepos.GetByIdNoTracking(editVM.Id);
            if (user.ProfileImageryUrl == "" || user.ProfileImageryUrl == null)
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
                    await _photoService.DeletePhotoAsync(user.ProfileImageryUrl);
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
