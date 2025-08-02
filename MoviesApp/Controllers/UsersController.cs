using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.Business.Repos.Interfaces;
using Movies.DataAccess.Data;
using Movies.DataAccess.ViewModels;
using Movies.Models;
using MoviesApp.Services;
using MoviesApp.Services.Interfaces;

namespace MoviesApp.Controllers
{
    public class UsersController : ControllerBase
    {
        private readonly IWebApiExecutor _webApiExecutor;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContextAcessor;

        public UsersController(
            IWebApiExecutor webApiExecutor
            , IPhotoService photoService
            , IHttpContextAccessor httpContextAccessor)
        {
            _webApiExecutor = webApiExecutor;
            _photoService = photoService;
            _httpContextAcessor = httpContextAccessor;
        }

        [HttpGet("Users")]
        public async Task<IActionResult> Index()
        {
            var users = await _webApiExecutor.InvokeGet<List<AppUserDto>>("Users/GetAll");

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
            
            try
            {
                var user = await _webApiExecutor.InvokeGet<AppUserDto>($"Users/GetById/{userId}");
                var userPlaylists = await _webApiExecutor.InvokeGet<List<PlaylistDto>>($"Users/GetAllPlaylists/{userId}");

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
            catch (WebApiException ex)
            {
                HandleApiException(ex);
                //TempData["error"] = "API exception. " + ex.Response.ErrorMessage;
                TempData["error"] = "Api exception: " + (ex.ErrorResponse?.Errors != null
                        ? string.Join("; ", ex.ErrorResponse.Errors.SelectMany(e => e.Value))
                        : "Unknown error");
            }
            return RedirectToAction("Index");
        }

        // GET: Movies/EditProfile/5 -----------------------------------------------------------------

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            //var user = await _userService.GetById(id);
            var user = await _webApiExecutor.InvokeGet<AppUserDto>($"Users/GetById/{id}");
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

            //var userDto = await _userService.GetByIdNoTracking(id);
            var userDto = await _webApiExecutor.InvokeGet<AppUserDto>($"Users/GetByIdNoTracking/{id}");

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
                await _webApiExecutor.InvokePut($"Users/Put/{id}", userDto);
                TempData["success"] = "User info updated";
            }
            catch (WebApiException ex)
            {
                HandleApiException(ex);
                //TempData["error"] = "API exception: " + ex.Response.ErrorMessage;
                TempData["error"] = "Api exception: " + (ex.ErrorResponse?.Errors != null
                        ? string.Join("; ", ex.ErrorResponse.Errors.SelectMany(e => e.Value))
                        : "Unknown error");
                return View("Edit", editVM);
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _webApiExecutor.InvokeGet<AppUserDto>($"Users/GetById/{id}");
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
            try { 
                var user = await _webApiExecutor.InvokeGet<AppUserDto>($"Users/GetById/{id}");
                if (user == null)
                {
                    return View("Error");
                }
                await _webApiExecutor.InvokeDelete($"Users/Delete/{id}");
                TempData["success"] = "User deleted";
            } catch (WebApiException ex)
            {
                HandleApiException(ex);
                //TempData["error"] = "API exception: " + ex.Response.ErrorMessage;
                TempData["error"] = "Api exception: " + (ex.ErrorResponse?.Errors != null
                        ? string.Join("; ", ex.ErrorResponse.Errors.SelectMany(e => e.Value))
                        : "Unknown error");
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
