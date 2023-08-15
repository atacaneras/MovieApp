using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieApp.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Pages
{
    public class DetailsModel : PageModel
    {
        public Result SelectedMovie { get; set; }
        public List<Video> MovieVideos { get; set; } = new List<Video>();
        public List<Result> SimilarMovies { get; set; } = new List<Result>();
        public class Video
        {
            public string Key { get; set; }
            public string Name { get; set; }
            public string FullUrl => $"https://www.youtube.com/watch?v={Key}";
        }
        public class VideoSearchResult
        {
            public List<VideoInfo> results { get; set; }

            public class VideoInfo
            {
                public string key { get; set; }
                public string name { get; set; }
            }
        }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            await GetMovieDetails(id);
            await GetSimilarMovies(id);
            return Page();
        }
        private async Task GetMovieDetails(int id)
        {
            try
            {
                var options = new RestClientOptions($"https://api.themoviedb.org/3/movie/{id}");
                var client = new RestClient(options);
                var request = new RestRequest("");
                request.AddHeader("accept", "application/json");
                request.AddHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiJiYzNjZjJmMmJmMTdmYmY0MmQxZmY2ZGNjMGJmZDY0NyIsInN1YiI6IjY0ZDI1ZGRmNTQ5ZGRhMDExYzI5YjE3YSIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.qTpCDEqFXSnb-Apj_aD_PMycKfPdxVDewqvwQJg2aqI"); // Replace with your API key
                var response = await client.GetAsync(request);

                var output = response.Content;
                var deserializedMovie = JsonConvert.DeserializeObject<Result>(output);

                SelectedMovie = deserializedMovie;

                await GetMovieVideos(id);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching movie details: {ex.Message}");
                SelectedMovie = null;
                MovieVideos = new List<Video>();
            }
        }
        private async Task GetSimilarMovies(int id)
        {
            try
            {
                var options = new RestClientOptions($"https://api.themoviedb.org/3/movie/{id}/similar");
                var client = new RestClient(options);
                var request = new RestRequest("");
                request.AddHeader("accept", "application/json");
                request.AddHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiJiYzNjZjJmMmJmMTdmYmY0MmQxZmY2ZGNjMGJmZDY0NyIsInN1YiI6IjY0ZDI1ZGRmNTQ5ZGRhMDExYzI5YjE3YSIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.qTpCDEqFXSnb-Apj_aD_PMycKfPdxVDewqvwQJg2aqI");
                var response = await client.GetAsync(request);

                var output = response.Content;
                var deserializedMovies = JsonConvert.DeserializeObject<MovieSearchResult>(output);

                SimilarMovies = deserializedMovies.results;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching similar movies: {ex.Message}");
                SimilarMovies = new List<Result>();
            }
        }
        private async Task GetMovieVideos(int id)
        {
            try
            {
                var options = new RestClientOptions($"https://api.themoviedb.org/3/movie/{id}/videos");
                var client = new RestClient(options);
                var request = new RestRequest("");
                request.AddHeader("accept", "application/json");
                request.AddHeader("Authorization", "Bearer eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiJiYzNjZjJmMmJmMTdmYmY0MmQxZmY2ZGNjMGJmZDY0NyIsInN1YiI6IjY0ZDI1ZGRmNTQ5ZGRhMDExYzI5YjE3YSIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.qTpCDEqFXSnb-Apj_aD_PMycKfPdxVDewqvwQJg2aqI");
                var response = await client.GetAsync(request);

                var output = response.Content;
                var deserializedVideos = JsonConvert.DeserializeObject<VideoSearchResult>(output);

                if (deserializedVideos.results != null)
                {
                    MovieVideos = deserializedVideos.results.Select(v => new Video { Key = v.key, Name = v.name }).ToList();

                    foreach (var video in MovieVideos)
                    {
                        Console.WriteLine($"Video Name: {video.Name}, Key: {video.Key}");
                    }
                }
                else
                {
                    MovieVideos = new List<Video>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching movie videos: {ex.Message}");
                MovieVideos = new List<Video>();
            }
        }
    }
}