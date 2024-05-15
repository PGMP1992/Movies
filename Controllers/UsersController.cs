﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MoviesApp.Interfaces;
using MoviesApp.Models;
using MoviesApp.ViewModels;

namespace MoviesApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersRepos _userRepos;
        private readonly IPhotoService _photoService;
        private readonly UserManager<AppUser> _userManager;

        public UsersController(IUsersRepos usersRepos, IPhotoService photoService, UserManager<AppUser> userManager)
        {
            _userRepos = usersRepos;
            _photoService = photoService;
            _userManager = userManager;
        }

        [HttpGet("Users")]
        public async Task<IActionResult> Index()
        {
            //if ("admin")
            //{
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
            //}
            //else
            //{
            //    var user = await _userRepos.GetUserById(id);

            //    var usersVM = new UsersVM()
            //    {
            //        Id = user.Id,
            //        UserName = user.UserName,
            //        City = user.City,
            //        State = user.State,
            //        ProfileImageUrl = user.ProfileImageryUrl,
            //    };
            //    //result.Add(usersVM);
            //}
            //return View(result);
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

        // GET: Movies/Edit/5
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return View("Error");
            }

            var editVM = new EditProfileVM()
            {
                City = user.City,
                State = user.State,
                ProfileImageUrl = user.ProfileImageryUrl,
            };
            return View(editVM);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileVM editVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit profile");
                return View("Edit", editVM);
            }

            var user = await _userManager.GetUserAsync(User);

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

        // GET: Movies/Edit/5
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

        //        await _userRepos.UpdateAsync(user);
        //    }

        //    user.City = editVM.City;
        //    user.State = editVM.State;

        //    await _userRepos.UpdateAsync(user);

        //    return RedirectToAction(nameof(Index));
        //}
    }
}
