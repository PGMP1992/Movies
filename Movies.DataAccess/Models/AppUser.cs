using Microsoft.AspNetCore.Identity;
using Movies.Models;

namespace Movies.DataAccess.Models
{
    public class AppUser : IdentityUser
    {
        public string? ProfileImageryUrl { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }

        public AppUserDto ToDto()
        {
            return new AppUserDto
            {
                Id = this.Id,
                City = this.City,
                State = this.State,
                ProfileImageryUrl = this.ProfileImageryUrl,

                UserName = this.UserName,
                NormalizedUserName = this.NormalizedUserName,
                Email = this.Email,
                NormalizedEmail = this.NormalizedEmail,
                EmailConfirmed = this.EmailConfirmed,
                PasswordHash = this.PasswordHash,
                SecurityStamp = this.SecurityStamp,
                ConcurrencyStamp = this.ConcurrencyStamp,
                PhoneNumber = this.PhoneNumber,
                PhoneNumberConfirmed = this.PhoneNumberConfirmed,
                TwoFactorEnabled = this.TwoFactorEnabled,
                LockoutEnd = this.LockoutEnd,
                LockoutEnabled = this.LockoutEnabled,
                AccessFailedCount = this.AccessFailedCount
            };
        }
    }
}
