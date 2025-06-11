using Microsoft.AspNetCore.Identity;

namespace Movies.Models
{
    public class AppUserDto : IdentityUser
    {
        public string? ProfileImageryUrl { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? UserName { get; set; }

        //dbUser.Id = user.Id;
        //    dbUser.UserName = user.UserName;
        //    dbUser.NormalizedUserName = user.NormalizedUserName;
        //    dbUser.Email = user.Email;
        //    dbUser.NormalizedEmail = user.NormalizedEmail;

        //    dbUser.City = user.City;
        //    dbUser.State = user.State;
        //    dbUser.ProfileImageryUrl = user.ProfileImageryUrl;

        //    dbUser.EmailConfirmed = user.EmailConfirmed;
        //    dbUser.PasswordHash = user.PasswordHash;
        //    dbUser.SecurityStamp = user.SecurityStamp;
        //    dbUser.ConcurrencyStamp = user.ConcurrencyStamp;
        //    dbUser.PhoneNumber = user.PhoneNumber;
        //    dbUser.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
        //    dbUser.TwoFactorEnabled = user.TwoFactorEnabled;
        //    dbUser.LockoutEnd = user.LockoutEnd;
        //    dbUser.LockoutEnabled = user.LockoutEnabled;
        //    dbUser.AccessFailedCount = user.AccessFailedCount;
    }
}
