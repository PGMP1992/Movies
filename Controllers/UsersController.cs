using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MoviesApp.Data;
using MoviesApp.Models;
using MoviesApp.Repos.Interfaces;
using MoviesApp.ViewModels;

namespace MoviesApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersRepos _userRepos;
        private readonly IPhotoService _photoService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAcessor;

        public UsersController(
            IUsersRepos usersRepos
            , IPhotoService photoService
            , UserManager<AppUser> userManager
            , IHttpContextAccessor httpContextAccessor)
        {
            _userRepos = usersRepos;
            _photoService = photoService;
            _userManager = userManager;
            _httpContextAcessor = httpContextAccessor;
        }

        [HttpGet("Users")]
        public async Task<IActionResult> Index()
        {
            var users = await _userRepos.GetAll();
            List<UsersVM> result = new List<UsersVM>();

            foreach (var user in users)
            {
                var usersVM = new UsersVM()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    City = user.City,
                    State = user.State,
                    ProfileImageUrl = user.ProfileImageryUrl,
                };
                result.Add(usersVM);
            }
            return View(result);
        }

        [Authorize]
        public async Task<IActionResult> Detail(string? id)
        {
            var userId = string.Empty;
            // Ensure HttpContext and User are not null before accessing them
            if (id == null)
            {
                var httpContext = _httpContextAcessor.HttpContext;
                var curUserId = httpContext.User.GetUserId();

                if (httpContext == null || httpContext.User == null)
                {
                    return BadRequest("Unable to retrieve user information.");
                }

                if (curUserId == null)
                {
                    return BadRequest("User ID is null.");
                }
                userId = curUserId;
            }
            else
            {
                userId = id;
            }
                    
            var user = await _userRepos.GetById(userId);
            var userPlaylists = await _userRepos.GetAllPlaylists(userId);

            var userDetailsVM = new UsersDetailsVM()
            {
                Playlists = userPlaylists,
                Id = userId,
                UserName = user.UserName,
                City = user.City,
                State = user.State,
                ProfileImageUrl = user.ProfileImageryUrl
            };

            return View(userDetailsVM);
        }


        // GET: Movies/EditProfile/5 -----------------------------------------------------------------

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userRepos.GetById(id);
            if (user == null)
            {
                return View("Error");
            }

            var editUserVM = new EditUserVM()
            {
                Id = user.Id,
                UserName = user.UserName,
                City = user.City,
                State = user.State,
                ProfileImageUrl = user.ProfileImageryUrl
            };
            return View(editUserVM);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(string id, EditUserVM editVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit profile");
                return View("Edit", editVM);
            }

            var user = await _userRepos.GetById(id);

            if (user == null)
            {
                return View("Error");
            }

            if (editVM.Image != null) // only update profile image
            {
                var photoResult = await _photoService.AddPhotoAsync(editVM.Image);

                if (photoResult.Error != null)
                {
                    ModelState.AddModelError("Image", "Failed to upload image");
                    return View("Edit", editVM);
                }

                if (!string.IsNullOrEmpty(user.ProfileImageryUrl))
                {
                    _ = _photoService.DeletePhotoAsync(user.ProfileImageryUrl);
                }

                user.ProfileImageryUrl = photoResult.Url.ToString();
                editVM.ProfileImageUrl = user.ProfileImageryUrl;

                //await _userManager.UpdateAsync(user);

                //return View(editVM);
            }

            user.City = editVM.City;
            user.State = editVM.State;

            await _userManager.UpdateAsync(user);
            TempData["success"] = "User info updated";

            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> Delete (string id)
        {
            var user = await _userRepos.GetById(id);
            if (user == null)
            {
                return View("Error");
            }

            var editUserVM = new EditUserVM()
            {
                Id = user.Id,
                UserName = user.UserName,
                City = user.City,
                State = user.State,
                ProfileImageUrl = user.ProfileImageryUrl,
            };
            return View(editUserVM);
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(string id)
        {
            var user = await _userRepos.GetById(id);
            if (user == null)
            {
                return View("Error");
            }
            _userRepos.Delete(user);
            TempData["success"] = "User deleted";
            return RedirectToAction(nameof(Index));
        }
    }
}
