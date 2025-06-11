using Movies.Models;
using MoviesApp.Services.Interfaces;
using Newtonsoft.Json;

namespace MoviesApp.Services
{
    public class UserService : IUserService
    {
        //Uri baseAddress = new Uri("https://localhost:7231/api");
        private readonly HttpClient _httpClient;
        private IConfiguration _configuration;
        private readonly string BaseServerUrl;

        public UserService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            BaseServerUrl = _configuration.GetSection("BaseServerUrl").Value;
            _httpClient.BaseAddress = new Uri(BaseServerUrl);
        }

        public async Task<IEnumerable<AppUserDto>> GetAll()
        {
            var response = await _httpClient.GetAsync("/api/users/GetAll");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<IEnumerable<AppUserDto>>(content);
                return users ?? new List<AppUserDto>();
            }

            return new List<AppUserDto>();
        }

        public async Task<AppUserDto> GetById(string id)
        {
            var response = await _httpClient.GetAsync($"/api/users/GetById/{id}");
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var user = JsonConvert.DeserializeObject<AppUserDto>(content);
                return user ?? new AppUserDto();
            }
            else
            {
                var errorModel = JsonConvert.DeserializeObject<ErrorModelDto>(content) ?? new ErrorModelDto();
                throw new Exception(errorModel.ErrorMessage);
            }
        }

        public async Task<AppUserDto> GetByIdNoTracking(string id)
        {
            var response = await _httpClient.GetAsync($"/api/users/GetByIdNoTracking/{id}");
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var user = JsonConvert.DeserializeObject<AppUserDto>(content);
                return user ?? new AppUserDto();
            }
            else
            {
                var errorModel = JsonConvert.DeserializeObject<ErrorModelDto>(content) ?? new ErrorModelDto();
                throw new Exception(errorModel.ErrorMessage);
            }
        }

        public async Task<IEnumerable<PlaylistDto>> GetAllPlaylists(string id)
        {
            var response = await _httpClient.GetAsync($"/api/users/GetAllPlaylists/{id}");
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var playlists = JsonConvert.DeserializeObject<IEnumerable<PlaylistDto>>(content);
                return playlists ?? new List<PlaylistDto>();
            }
            else
            {
                var errorModel = JsonConvert.DeserializeObject<ErrorModelDto>(content) ?? new ErrorModelDto();
                throw new Exception(errorModel.ErrorMessage);
            }
        }

        public async Task<AppUserDto> Update(string id, AppUserDto user)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/users/put/{id}", user);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var userResponse = JsonConvert.DeserializeObject<AppUserDto>(content);
                return userResponse;
            }
            else
            {
                var errorModel = JsonConvert.DeserializeObject<ErrorModelDto>(content) ?? new ErrorModelDto();
                throw new Exception(errorModel.ErrorMessage);
            }
        }

        public async Task<bool> Delete(string id)
        {
            var response = await _httpClient.DeleteAsync($"/api/users/Delete/{id}");
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                var errorModel = JsonConvert.DeserializeObject<ErrorModelDto>(content) ?? new ErrorModelDto();
                throw new Exception(errorModel.ErrorMessage);
            }
        }
    }
}
