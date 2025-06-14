﻿using Microsoft.AspNetCore.Identity;

namespace Movies.Models
{
    public class AppUserDto : IdentityUser
    {
        public string? ProfileImageryUrl { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
    }
}
