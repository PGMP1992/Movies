using MoviesApp.DTOs;
using Newtonsoft.Json;

namespace MoviesApp.Services
{
    public class MovieService : IMovieService
    {
        Uri baseAddress = new Uri("https://localhost:7231/api");
        private readonly HttpClient _httpClient;
        private IConfiguration _configuration;
        private string BaseServerUrl;
        
        public MovieService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = baseAddress;
            _configuration =configuration;
            //BaseServerUrl = _configuration.GetSection("BaseServerUrl").Value;
            BaseServerUrl = _configuration.GetSection("BaseServerUrl").Value;
        }

        public async Task<IEnumerable<MovieDto>> GetAll()
        {
            //var response = await _httpClient.GetAsync("/api/movies/GetAll");
            var response = await _httpClient.GetAsync(_httpClient.BaseAddress + "/movies/GetAll");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var movies = JsonConvert.DeserializeObject<IEnumerable<MovieDto>>(content);
                //foreach(var prod in products)
                //{
                //    prod.ImageUrl=BaseServerUrl+prod.ImageUrl;
                //}
                return movies;
            }

            return new List<MovieDto>();
        }
        
        public async Task<MovieDto> Get(int id)
        {
            var response = await _httpClient.GetAsync($"/api/movie/{id}");
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                var movie = JsonConvert.DeserializeObject<MovieDto>(data);
                //product.ImageUrl=BaseServerUrl+product.ImageUrl;
                return movie;
            }
            else
            {
                var errorModel = JsonConvert.DeserializeObject<ErrorModelDTO>(content);
                throw new Exception(errorModel.ErrorMessage);
            }
        }

        public async Task<MovieDto> Add(MovieDto movie)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MovieDto>> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public async Task<MovieDto> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<MovieDto> Update(MovieDto movie)
        {
            throw new NotImplementedException();
        }
    }
}
