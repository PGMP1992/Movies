using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MoviesApp.Models;
using MoviesApp.Models.ViewModels;
using MoviesApp.Repos.Interfaces;


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
            var users = await _userRepos.GetAll();
            List<UserVM> result = new List<UserVM>();

            foreach (var user in users)
            {
                var usersVM = new UserVM()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    City = user.City,
                    State = user.State,
                    ImageUrl = user.ImageUrl,
                };
                result.Add(usersVM);
            }
            return View(result);
        }

        [Authorize]
        public async Task<ActionResult> Detail(string id)
        {
            var user = await _userRepos.GetById(id);
            if (user == null)
            {
                return RedirectToAction("Index", "Users");
            }

            var usersDetailsVM = new UserVM()
            {
                Id = user.Id,
                UserName = user.UserName,
                City = user.City,
                State = user.State,
                ImageUrl = user.ImageUrl ?? "/img/avatar-male-4.jpg",
            };
            return View(usersDetailsVM);
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

            var editUserVM = new UserVM()
            {
                Id = user.Id,
                UserName = user.UserName,
                City = user.City,
                State = user.State,
                ImageUrl = user.ImageUrl
            };
            return View(editUserVM);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(string id, UserVM editVM)
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

                if (!string.IsNullOrEmpty(user.ImageUrl))
                {
                    _ = _photoService.DeletePhotoAsync(user.ImageUrl);
                }

                user.ImageUrl = photoResult.Url.ToString();
                editVM.ImageUrl = user.ImageUrl;

                //await _userManager.UpdateAsync(user);

                //return View(editVM);
            }

            user.City = editVM.City;
            user.State = editVM.State;

            await _userManager.UpdateAsync(user);

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

            var editUserVM = new UserVM()
            {
                Id = user.Id,
                UserName = user.UserName,
                City = user.City,
                State = user.State,
                ImageUrl = user.ImageUrl,
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
            return RedirectToAction(nameof(Index));
        }
    }
}
