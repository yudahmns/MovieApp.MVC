using MovieApp.MVC.Models;
using System.Text.Json;
using System.Text;
using System.Net.Mime;

namespace MovieApp.MVC.Services
{
    public sealed class MovieService
    {
        private readonly HttpClient _client;

        public MovieService(HttpClient client)
        {
            if (client is null)
                throw new ArgumentNullException(nameof(client));
            client.BaseAddress = new Uri("https://localhost:7088");
            _client = client;
        }

        public async Task Create(Movie movie)
        {
            using StringContent json = new(JsonSerializer.Serialize(movie, new JsonSerializerOptions(JsonSerializerDefaults.Web)), Encoding.UTF8, MediaTypeNames.Application.Json);
            using HttpResponseMessage httpResponse = await _client.PostAsync("/api/Movies", json);
            httpResponse.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<Movie>> Read()
        {
            var response = await _client.GetFromJsonAsync<Movie[]>("/api/Movies", new JsonSerializerOptions(JsonSerializerDefaults.Web));
            return response ?? (new Movie[]{});
        }

        public async Task<Movie?> Read(int id)
        {
            var response = await _client.GetFromJsonAsync<Movie>($"/api/Movies/{id}", new JsonSerializerOptions(JsonSerializerDefaults.Web));
            return response;
        }

        public async Task Update(int id, Movie movie)
        {
            using StringContent json = new(JsonSerializer.Serialize(movie, new JsonSerializerOptions(JsonSerializerDefaults.Web)), Encoding.UTF8, MediaTypeNames.Application.Json);
            using HttpResponseMessage httpResponse = await _client.PutAsync($"/api/Movies/{movie.Id}", json);
            httpResponse.EnsureSuccessStatusCode();
        }        

        public async Task Delete(int id)
        {
            using HttpResponseMessage httpResponse = await _client.DeleteAsync($"/api/Movies/{id}");
            httpResponse.EnsureSuccessStatusCode();
        }

    }
}