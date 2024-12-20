﻿namespace MoviesApp.Models.ViewModels
{
    public class EditUserDashboardVM
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string? ImageUrl { get; set; } = string.Empty;
        public IFormFile Image { get; set; }
    }
}
