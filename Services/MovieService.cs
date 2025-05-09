using Azure;
using Movies.Models;
using Movies.Models.ViewModels;
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

        public async Task<IEnumerable<MovieDto>> GetAllActive()
        {
            var response = await _httpClient.GetAsync(_httpClient.BaseAddress + $"/movies/GetAllActive");
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

        public async Task<IEnumerable<MovieDto>> GetByName(string name)
        {
            var response = await _httpClient.GetAsync(_httpClient.BaseAddress + $"/movies/GetByName/{name}");
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
            var response = await _httpClient.GetAsync(_httpClient.BaseAddress + $"/movies/Get/{id}");

            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var movie = JsonConvert.DeserializeObject<MovieDto>(content);
                return movie;
            }
            else
            {
                var errorModel = JsonConvert.DeserializeObject<ErrorModelDTO>(content);
                throw new Exception(errorModel.ErrorMessage);
            }
        }

        public async Task<MovieDto> Update(MovieDto movie)
        {
            var response = await _httpClient.PutAsJsonAsync(_httpClient.BaseAddress + "/movies/Update", movie);

            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                movie = JsonConvert.DeserializeObject<MovieDto>(content);
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
            var response = await _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + "/movies/Post", movie);

            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                movie = JsonConvert.DeserializeObject<MovieDto>(content);
                return movie;
            }
            else
            {
                var errorModel = JsonConvert.DeserializeObject<ErrorModelDTO>(content);
                throw new Exception(errorModel.ErrorMessage);
            }
        }

        public async Task<bool> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync(_httpClient.BaseAddress + $"/movies/Delete/{id}");

            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                //movie = JsonConvert.DeserializeObject<MovieDto>(content);
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
