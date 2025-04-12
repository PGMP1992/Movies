using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoviesApp.Data;
using MoviesApp.Repos.Interfaces;
using MoviesApp.ViewModels;

namespace MoviesApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IUsersRepos _usersRepos;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPhotoService _photoService;

        public DashboardController(IUsersRepos usersRepos,
                                    IHttpContextAccessor httpContextAccessor,
                                    IPhotoService photoService)
        {
            _usersRepos = usersRepos;
            _httpContextAccessor = httpContextAccessor;
            _photoService = photoService;
        }

        //[Authorize]
        //private void MapUserEdit(AppUser user, EditUserVM editVM, ImageUploadResult photoResult)
        //{
        //    user.Id = editVM.Id;
        //    user.ProfileImageryUrl = photoResult.Url.ToString();
        //}

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var user = await _usersRepos.GetUserById(curUserId);

            var userPlaylists = await _usersRepos.GetAllUserPlaylists();

            var userDetailsVM = new UsersDetailsVM()
            {
                Playlists = userPlaylists,
                Id = curUserId,
                UserName = user.UserName,
                City = user.City,
                State = user.State,
                ProfileImageUrl = user.ProfileImageryUrl
            };

            return View(userDetailsVM);
        }

        //[Authorize]
        //public async Task<IActionResult> EditUserProfile()
        //{
        //    var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
        //    var user = await _dashboardRepos.GetUserById(curUserId);

        //    if (user == null) return View("Error");

        //    var editUserVM = new EditUserVM()
        //    {
        //        Id = curUserId,
        //        UserName = user.UserName,
        //        City = user.City,
        //        State = user.State,
        //        ProfileImageUrl = user.ProfileImageryUrl
        //    };

        //    return View(editUserVM);
        //}

        //[Authorize]
        //[HttpPost]
        //public async Task<IActionResult> editUserProfile(EditUserVM editVM)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        ModelState.AddModelError("", "Failed to Edit Profile");
        //        return View("EditUserProfile", editVM);
        //    }

        //    var user = await _dashboardRepos.GetByIdNoTracking(editVM.Id);

        //    if (user.ProfileImageryUrl == "" || user.ProfileImageryUrl == null)
        //    {
        //        var photoResult = await _photoService.AddPhotoAsync(editVM.Image);
        //        MapUserEdit(user, editVM, photoResult);

        //        // Optmistic Concurrency - 
        //        _dashboardRepos.Update(user);
        //        return RedirectToAction("index");
        //    }
        //    else
        //    {
        //        try
        //        {
        //            if(editVM.ProfileImageUrl != user.ProfileImageryUrl)
        //            {
        //                // This is wrong. Should have a check if it is different image
        //                await _photoService.DeletePhotoAsync(user.ProfileImageryUrl);
        //                var photoResult = await _photoService.AddPhotoAsync(editVM.Image);
        //                MapUserEdit(user, editVM, photoResult);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ModelState.AddModelError("", "Could not delete photo");
        //            return View(editVM);
        //        }

        //        user.City = editVM.City;
        //        user.State = editVM.State;

        //        // Optmistic Concurrency - 
        //        _dashboardRepos.Update(user);
        //        return RedirectToAction("Index");
        //    }
        //}
    }
}
