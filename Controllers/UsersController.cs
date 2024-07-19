using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public UsersController(IUsersRepos usersRepos, IPhotoService photoService,
            UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userRepos = usersRepos;
            _photoService = photoService;
            _userManager = userManager;
            _httpContextAcessor = httpContextAccessor;
        }

        [HttpGet("Users")]
        public async Task<IActionResult> Index()
        {
            var users = await _userRepos.GetAllUsers();
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

        public async Task<ActionResult> Detail(string id)
        {
            var user = await _userRepos.GetUserById(id);
            if (user == null)
            {
                return RedirectToAction("Index", "Users");
            }

            var usersDetailsVM = new UsersDetailsVM()
            {
                Id = user.Id,
                UserName = user.UserName,
                City = user.City,
                State = user.State,
                ProfileImageUrl = user.ProfileImageryUrl ?? "/img/avatar-male-4.jpg",
            };
            return View(usersDetailsVM);
        }

        // GET: Movies/EditProfile/5 -----------------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userRepos.GetUserById(id);
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

        [HttpPost]
        public async Task<IActionResult> Edit(string id, EditUserVM editVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit profile");
                return View("Edit", editVM);
            }

            var user = await _userRepos.GetUserById(id);

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

                await _userManager.UpdateAsync(user);

                //return View(editVM);
            }

            user.City = editVM.City;
            user.State = editVM.State;

            await _userManager.UpdateAsync(user);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete (string id)
        {
            var user = await _userRepos.GetUserById(id);
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

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(string id)
        {
            var user = await _userRepos.GetUserById(id);
            if (user == null)
            {
                return View("Error");
            }
            _userRepos.Delete(user);
            return RedirectToAction(nameof(Index));
        }

        // GET: Movies/Edit/5 -----------------------------------------------------------------
        //[HttpGet]
        //public async Task<IActionResult> Edit(string id)
        //{
        //    var user = await _userRepos.GetUserById(id);

        //    if (user == null)
        //    {
        //        return View("Error");
        //    }

        //    var editVM = new EditUserVM()
        //    {
        //        City = user.City,
        //        State = user.State,
        //        ProfileImageUrl = user.ProfileImageryUrl,
        //    };
        //    return View(editVM);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Edit(string id, EditUserVM editVM)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        ModelState.AddModelError("", "Failed to edit profile");
        //        return View("Edit", editVM);
        //    }

        //    var user = await _userRepos.GetUserById(id);

        //    if (user == null)
        //    {
        //        return View("Error");
        //    }

        //    if (editVM.Image != null) // only update profile image
        //    {
        //        var photoResult = await _photoService.AddPhotoAsync(editVM.Image);

        //        if (photoResult.Error != null)
        //        {
        //            ModelState.AddModelError("Image", "Failed to upload image");
        //            return View("Edit", editVM);
        //        }

        //        if (!string.IsNullOrEmpty(user.ProfileImageryUrl))
        //        {
        //            _ = _photoService.DeletePhotoAsync(user.ProfileImageryUrl);
        //        }

        //        user.ProfileImageryUrl = photoResult.Url.ToString();
        //        editVM.ProfileImageUrl = user.ProfileImageryUrl;

        //        //_userRepos.Update(user);
        //    }

        //    user.City = editVM.City;
        //    user.State = editVM.State;

        //    _userRepos.Update(user);

        //    return RedirectToAction(nameof(Index));
        //}
    }
}
