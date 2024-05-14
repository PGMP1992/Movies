using Microsoft.AspNetCore.Mvc;
using MoviesApp.Interfaces;
using MoviesApp.Models;
using MoviesApp.ViewModels;

namespace MoviesApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersRepos _userRepos;

        public UsersController(IUsersRepos usersRepos)
        {
            _userRepos = usersRepos;
        }

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
                    ProfileImageUrl = user.ProfileImageryUrl,
                    City = user.City,
                    State = user.State
                };
                result.Add(usersVM);
            }
            return View(result);
        }

        public async Task<ActionResult> Detail(string id)
        {
            var user = await _userRepos.GetUserById(id);
            var usersDetailsVM = new UsersDetailsVM()
            {
                Id = user.Id,
                UserName = user.UserName,
            };
            return View(usersDetailsVM);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userRepos.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            var userVM = new UsersDetailsVM()
            {
                Id  = user.Id,
                UserName = user.UserName,
                PictUrl = user.ProfileImageryUrl,
                City = user.City,
                State = user.State
            };

            return View(userVM);
        }
    }
}
