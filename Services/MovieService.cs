using Newtonsoft.Json;
using System.Net.NetworkInformation;
using MoviesApp.DTOs;

namespace MoviesApp.Services
{
    public class MovieService : IMovieService
    {
        //Uri baseAddress = new Uri("https://localhost:7231/api");
        private readonly HttpClient _httpClient;
        private IConfiguration _configuration;
        private string BaseServerUrl;

        public MovieService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            BaseServerUrl = _configuration.GetSection("BaseServerUrl").Value;
            _httpClient.BaseAddress = new Uri(BaseServerUrl);
        }

        public async Task<IEnumerable<MovieDto>> GetAll()
        {
            var response = await _httpClient.GetAsync("/api/movies/Get");
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var movies = JsonConvert.DeserializeObject<IEnumerable<MovieDto>>(content);
                return movies;
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
                return movies;
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
                return movies;
            }
            
            return new List<MovieDto>();
        }

        public async Task<MovieDto> Get(int id)
        {
            var response = await _httpClient.GetAsync($"/api/movies/{id}");
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var movie = JsonConvert.DeserializeObject<MovieDto>(content);
                return movie;
            }
            else
            {
                var errorModel = JsonConvert.DeserializeObject<ErrorModel>(content);
                throw new Exception(errorModel.ErrorMessage);
            }
        }

        public async Task<MovieDto> Add(MovieDto movie)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/movies/Post", movie);

            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var movieResponse = JsonConvert.DeserializeObject<MovieDto>(content);
                return movieResponse;
            }
            else
            {
                var errorModel = JsonConvert.DeserializeObject<ErrorModel>(content);
                throw new Exception(errorModel.ErrorMessage);
            }
        }

        public async Task<MovieDto> Update(MovieDto movie)
        {
            var response = await _httpClient.PutAsJsonAsync("/api/movies/Put", movie);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var movieResponse = JsonConvert.DeserializeObject<MovieDto>(content);
                return movieResponse;
            }
            else
            {
                var errorModel = JsonConvert.DeserializeObject<ErrorModel>(content);
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
            else {
                var errorModel = JsonConvert.DeserializeObject<ErrorModel>(content);
                throw new Exception(errorModel.ErrorMessage);
            }
            return false;
        }
    }
}
