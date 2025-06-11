using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Movies.Business.Repos.Interfaces;
using Movies.DataAccess.Data;
using Movies.DataAccess.Models;
using Movies.DataAccess.ViewModels;
using MoviesApp.Services.Interfaces;

namespace MoviesApp.Controllers
{
    public class UsersController : Controller
    {
        //private readonly IUserRepos _userRepos;
        private readonly IUserService _userService;
        private readonly IPhotoService _photoService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAcessor;

        public UsersController(
            //IUserRepos usersRepos
            IUserService userService
            , IPhotoService photoService
            , UserManager<AppUser> userManager
            , IHttpContextAccessor httpContextAccessor)
        {
            //_userRepos = usersRepos;
            _userService = userService;
            _photoService = photoService;
            _userManager = userManager;
            _httpContextAcessor = httpContextAccessor;
        }

        [HttpGet("Users")]
        public async Task<IActionResult> Index()
        {
            //var users = await _userRepos.GetAll();
            var users = await _userService.GetAll();
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

            //var user = await _userRepos.GetById(userId);
            //var userPlaylists = await _userRepos.GetAllPlaylists(userId);
            var user = await _userService.GetById(userId);
            var userPlaylists = await _userService.GetAllPlaylists(userId);

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
            //var user = await _userRepos.GetById(id);
            var user = await _userService.GetById(id);
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

            //var user = await _userRepos.GetById(id);
            var userDto = await _userService.GetByIdNoTracking(id);

            if (userDto == null)
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

                if (!string.IsNullOrEmpty(userDto.ProfileImageryUrl))
                {
                    _ = _photoService.DeletePhotoAsync(userDto.ProfileImageryUrl);
                }

                userDto.ProfileImageryUrl = photoResult.Url.ToString();
                editVM.ProfileImageUrl = userDto.ProfileImageryUrl;
            }

            userDto.City = editVM.City;
            userDto.State = editVM.State;

            try
            {
                //await _userManager.UpdateAsync(user);
                await _userService.Update(id, userDto);
                TempData["success"] = "User info updated";
            }
            catch (Exception ex)
            {
                TempData["error"] = $"Failed to update profile: {ex.Message}";
                return View("Edit", editVM);
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            //var user = await _userRepos.GetById(id);
            var user = await _userService.GetById(id);
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
            //var user = await _userRepos.GetById(id);
            var user = await _userService.GetById(id);
            if (user == null)
            {
                return View("Error");
            }
            //_userRepos.Delete(user);
            await _userService.Delete(id);
            TempData["success"] = "User deleted";
            return RedirectToAction(nameof(Index));
        }
    }
}
