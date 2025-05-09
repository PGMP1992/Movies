using Movies.Models;
using MoviesApp.DTOs;
using Newtonsoft.Json;

namespace MoviesApp.Services
{
    public class PlaylistService : IPlaylistService
    {
        //Uri baseAddress = new Uri("https://localhost:7231/api");
                
        private readonly HttpClient _httpClient;
        private IConfiguration _configuration;
        private string BaseServerUrl;

        public PlaylistService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            BaseServerUrl = _configuration.GetSection("BaseServerUrl").Value;
            _httpClient.BaseAddress = new Uri(BaseServerUrl);
        }

        public async Task<IEnumerable<PlaylistDto>> GetAll()
        {
            var response = await _httpClient.GetAsync("/api/playlists/GetAll");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var playlists = JsonConvert.DeserializeObject<IEnumerable<PlaylistDto>>(content);
                return playlists;
            }
            return new List<PlaylistDto>();
        }

        public async Task<IEnumerable<PlaylistDto>> GetAllByUser(string user)
        {
            var response = await _httpClient.GetAsync($"/api/playlists/GetAllByUser/{user}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var playlists = JsonConvert.DeserializeObject<IEnumerable<PlaylistDto>>(content);
                return playlists;
            }

            return new List<PlaylistDto>();
        }

        public async Task<PlaylistDto> Get(int id)
        {
            var response = await _httpClient.GetAsync($"/api/playlists/Get/{id}");

            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var playlist = JsonConvert.DeserializeObject<PlaylistDto>(content);
                return playlist;
            }
            else
            {
                var errorModel = JsonConvert.DeserializeObject<ErrorModelDTO>(content);
                throw new Exception(errorModel.ErrorMessage);
            }
        }

        public async Task<PlaylistDto> Update(PlaylistDto playlist)
        {
            var response = await _httpClient.PutAsJsonAsync("/api/playlists/Update", playlist);

            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                playlist = JsonConvert.DeserializeObject<PlaylistDto>(content);
                return playlist;
            }
            else
            {
                var errorModel = JsonConvert.DeserializeObject<ErrorModelDTO>(content);
                throw new Exception(errorModel.ErrorMessage);
            }
        }

        public async Task<PlaylistDto> Add(PlaylistDto playlist)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/playlists/Post", playlist);

            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                playlist = JsonConvert.DeserializeObject<PlaylistDto>(content);
                return playlist;
            }
            else
            {
                var errorModel = JsonConvert.DeserializeObject<ErrorModelDTO>(content);
                throw new Exception(errorModel.ErrorMessage);
            }
        }

        public async Task<bool> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/playlists/Delete/{id}");

            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                var errorModel = JsonConvert.DeserializeObject<ErrorModelDTO>(content);
                throw new Exception(errorModel.ErrorMessage);
            }
        }
    }
}
