using Movies.Models;
using Newtonsoft.Json;

namespace MoviesApp.Services
{
    public class MovieService : IMovieService
    {
        //Uri baseAddress = new Uri("https://localhost:7231/api");
        private readonly HttpClient _httpClient;
        private IConfiguration _configuration;
        private readonly string BaseServerUrl;

        public MovieService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            BaseServerUrl = _configuration.GetSection("BaseServerUrl").Value;
            _httpClient.BaseAddress = new Uri(BaseServerUrl);
        }

        public async Task<IEnumerable<MovieDto>> GetAll()
        {
            var response = await _httpClient.GetAsync("/api/movies/GetAll");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var movies = JsonConvert.DeserializeObject<IEnumerable<MovieDto>>(content);
                return movies ?? new List<MovieDto>();
            }

            return new List<MovieDto>();
        }

        public async Task<IEnumerable<MovieDto>> GetAllActive()
        {
            var response = await _httpClient.GetAsync("/api/movies/GetAllActive");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var movies = JsonConvert.DeserializeObject<IEnumerable<MovieDto>>(content);
                return movies ?? new List<MovieDto>();
            }

            return new List<MovieDto>();
        }

        public async Task<IEnumerable<MovieDto>> GetByName(string name)
        {
            var response = await _httpClient.GetAsync($"/api/movies/GetByName/{name}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var movies = JsonConvert.DeserializeObject<IEnumerable<MovieDto>>(content);
                return movies ?? new List<MovieDto>();
            }

            return new List<MovieDto>();
        }

        public async Task<MovieDto> GetById(int id)
        {
            var response = await _httpClient.GetAsync($"/api/movies/GetById/{id}");
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var movie = JsonConvert.DeserializeObject<MovieDto>(content);
                return movie ?? new MovieDto();
            }
            else
            {
                var errorModel = JsonConvert.DeserializeObject<ErrorModelDto>(content) ?? new ErrorModelDto();
                throw new Exception(errorModel.ErrorMessage);
            }
        }

        public async Task<MovieDto> GetByIdNoTracking(int id)
        {
            var response = await _httpClient.GetAsync($"/api/movies/GetByIdNoTracking/{id}");
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var movie = JsonConvert.DeserializeObject<MovieDto>(content);
                return movie ?? new MovieDto();
            }
            else
            {
                var errorModel = JsonConvert.DeserializeObject<ErrorModelDto>(content) ?? new ErrorModelDto();
                throw new Exception(errorModel.ErrorMessage);
            }
        }

        public async Task<MovieDto> Add(MovieDto movie)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/movies/post", movie);

            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var movieResponse = JsonConvert.DeserializeObject<MovieDto>(content);
                return movieResponse;
            }
            else
            {
                var errorModel = JsonConvert.DeserializeObject<ErrorModelDto>(content) ?? new ErrorModelDto();
                throw new Exception(errorModel.ErrorMessage);
            }
        }

        public async Task<MovieDto> Update(MovieDto movie)
        {
            var response = await _httpClient.PutAsJsonAsync("/api/movies/put/", movie);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var movieResponse = JsonConvert.DeserializeObject<MovieDto>(content);
                return movieResponse;
            }
            else
            {
                var errorModel = JsonConvert.DeserializeObject<ErrorModelDto>(content) ?? new ErrorModelDto();
                throw new Exception(errorModel.ErrorMessage);
            }
        }

        public async Task<bool> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/movies/Delete/{id}");
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
