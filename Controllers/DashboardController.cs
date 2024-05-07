﻿using Microsoft.AspNetCore.Mvc;
using MoviesApp.Data;
using MoviesApp.Interfaces;
using MoviesApp.ViewModels;
using MoviesApp.Models;
using MoviesApp.Repos;
using CloudinaryDotNet.Actions;

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

        private void MapUserEdit(AppUser user, EditUserDashboardVM editVM, ImageUploadResult photoResult)
        {
            user.Id = editVM.Id;
            user.ProfileImageryUrl = photoResult.Url.ToString();
        }

        public async Task<IActionResult> Index()
        {
            var userPlaylists = await _dashboardRepos.GetAllUserPlaylists();
            var userMovies = await _dashboardRepos.GetAllUserMovies();
            var dashboardVM = new DashboardVM()
            {
                Movies = userMovies,
                Playlists = userPlaylists
            };
            
            return View(dashboardVM);
        }

        public async Task<IActionResult> EditUserProfile()
        {
            var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var user = await _dashboardRepos.GetUserById(curUserId);
            
            if (user == null) return View("Error");
            
            var editUserVM = new EditUserDashboardVM()
            {
                Id = curUserId,
                ProfileImageUrl = user.ProfileImageryUrl
            };

            return View(editUserVM);
        }

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
                return RedirectToAction("index");
            }
        }


    }
}
