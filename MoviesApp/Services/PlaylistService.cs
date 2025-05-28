using Movies.Models;
using Newtonsoft.Json;

namespace MoviesApp.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly HttpClient _httpClient;
        private IConfiguration _configuration;
        private readonly string BaseServerUrl;

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
                var Playlists = JsonConvert.DeserializeObject<IEnumerable<PlaylistDto>>(content);
                return Playlists ?? new List<PlaylistDto>();
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
                return playlists ?? new List<PlaylistDto>();
            }

            return new List<PlaylistDto>();
        }

        public async Task<PlaylistDto> GetById(int id)
        {
            var response = await _httpClient.GetAsync($"/api/playlists/GetById/{id}");
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var playlist = JsonConvert.DeserializeObject<PlaylistDto>(content);
                return playlist ?? new PlaylistDto();
            }
            else
            {
                var errorModel = JsonConvert.DeserializeObject<ErrorModelDto>(content) ?? new ErrorModelDto();
                throw new Exception(errorModel.ErrorMessage);
            }
        }

        public async Task<PlaylistDto> GetByIdNoTracking(int id)
        {
            var response = await _httpClient.GetAsync($"/api/playlists/GetByIdNoTracking/{id}");
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var playlist = JsonConvert.DeserializeObject<PlaylistDto>(content);
                return playlist ?? new PlaylistDto();
            }
            else
            {
                var errorModel = JsonConvert.DeserializeObject<ErrorModelDto>(content) ?? new ErrorModelDto();
                throw new Exception(errorModel.ErrorMessage);
            }
        }

        public async Task<PlaylistDto> Add(PlaylistDto obj)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/playlists/post", obj);

            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var playlistResponse = JsonConvert.DeserializeObject<PlaylistDto>(content);
                return playlistResponse;
            }
            else
            {
                var errorModel = JsonConvert.DeserializeObject<ErrorModelDto>(content) ?? new ErrorModelDto();
                throw new Exception(errorModel.ErrorMessage);
            }
        }

        public async Task<PlaylistDto> Update(PlaylistDto obj)
        {
            var response = await _httpClient.PutAsJsonAsync("/api/playlists/put/", obj);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var playlistResponse = JsonConvert.DeserializeObject<PlaylistDto>(content);
                return playlistResponse;
            }
            else
            {
                var errorModel = JsonConvert.DeserializeObject<ErrorModelDto>(content) ?? new ErrorModelDto();
                throw new Exception(errorModel.ErrorMessage);
            }
        }

        public async Task<bool> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/playlists/delete/{id}");
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
