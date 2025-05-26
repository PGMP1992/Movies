using Movies.Models;
using Newtonsoft.Json;

namespace MoviesApp.Services
{
    public class PlaylistMovieService : IPlaylistMovieService
    {
        private readonly HttpClient _httpClient;
        private IConfiguration _configuration;
        private readonly string BaseServerUrl;

        public PlaylistMovieService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            BaseServerUrl = _configuration.GetSection("BaseServerUrl").Value;
            _httpClient.BaseAddress = new Uri(BaseServerUrl);
        }

        public async Task<IEnumerable<PlaylistMovieDto>> GetAll()
        {
            var response = await _httpClient.GetAsync("/api/playlistMovies/GetAll");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var Playlists = JsonConvert.DeserializeObject<IEnumerable<PlaylistMovieDto>>(content);
                return Playlists ?? new List<PlaylistMovieDto>();
            }

            return new List<PlaylistMovieDto>();
        }

        public async Task<IEnumerable<PlaylistMovieDto>> GetAllByUser(string user)
        {
            var response = await _httpClient.GetAsync($"/api/playlistMovies/GetAllByUser/{user}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var playlists = JsonConvert.DeserializeObject<IEnumerable<PlaylistMovieDto>>(content);
                return playlists ?? new List<PlaylistMovieDto>();
            }

            return new List<PlaylistMovieDto>();
        }

        public async Task<PlaylistMovieDto> GetById(int id)
        {
            var response = await _httpClient.GetAsync($"/api/playlistMovies/GetById/{id}");
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var playlist = JsonConvert.DeserializeObject<PlaylistMovieDto>(content);
                return playlist ?? new PlaylistMovieDto();
            }
            else
            {
                var errorModel = JsonConvert.DeserializeObject<ErrorModel>(content) ?? new ErrorModel();
                throw new Exception(errorModel.ErrorMessage);
            }
        }

        public async Task<PlaylistMovieDto> GetByIdNoTracking(int id)
        {
            var response = await _httpClient.GetAsync($"/api/playlistMovies/GetByIdNoTracking/{id}");
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var playlist = JsonConvert.DeserializeObject<PlaylistMovieDto>(content);
                return playlist ?? new PlaylistMovieDto();
            }
            else
            {
                var errorModel = JsonConvert.DeserializeObject<ErrorModel>(content) ?? new ErrorModel();
                throw new Exception(errorModel.ErrorMessage);
            }
        }

        public async Task<PlaylistMovieDto> Add(PlaylistMovieDto obj)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/playlistMovies/post", obj);

            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var playlistResponse = JsonConvert.DeserializeObject<PlaylistMovieDto>(content);
                return playlistResponse;
            }
            else
            {
                var errorModel = JsonConvert.DeserializeObject<ErrorModel>(content) ?? new ErrorModel();
                throw new Exception(errorModel.ErrorMessage);
            }
        }

        public async Task<PlaylistMovieDto> Update(PlaylistMovieDto obj)
        {
            var response = await _httpClient.PutAsJsonAsync("/api/playlistMovies/put/", obj);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var playlistResponse = JsonConvert.DeserializeObject<PlaylistMovieDto>(content);
                return playlistResponse;
            }
            else
            {
                var errorModel = JsonConvert.DeserializeObject<ErrorModel>(content) ?? new ErrorModel();
                throw new Exception(errorModel.ErrorMessage);
            }
        }

        public async Task<bool> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/playlistMovies/delete/{id}");
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                var errorModel = JsonConvert.DeserializeObject<ErrorModel>(content) ?? new ErrorModel();
                throw new Exception(errorModel.ErrorMessage);
            }
        }

        public async Task<IEnumerable<PlaylistMovieDto>> GetByPlaylist(int id)
        {
            var response = await _httpClient.GetAsync($"/api/playlistMovies/GetByPlaylist/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var Playlists = JsonConvert.DeserializeObject<IEnumerable<PlaylistMovieDto>>(content);
                return Playlists ?? new List<PlaylistMovieDto>();
            }

            return new List<PlaylistMovieDto>();
        }

        public async Task<PlaylistMovieDto?> GetByContent(int playlistId, int movieId)
        {
            var response = await _httpClient.GetAsync($"/api/playlistMovies/GetByContent/{playlistId}/{movieId}");

            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var playlistResponse = JsonConvert.DeserializeObject<PlaylistMovieDto>(content);
                return playlistResponse;
            }
            else
            {
                var errorModel = JsonConvert.DeserializeObject<ErrorModel>(content);
                return null;
                //throw new Exception(errorModel.ErrorMessage);
            }
        }

        public async Task<bool> MovieInPlaylist(int playlistId, int movieId)
        {
            var response = await _httpClient.GetAsync($"/api/playlistMovies/MovieInPlaylist/{playlistId}/{movieId}");
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
