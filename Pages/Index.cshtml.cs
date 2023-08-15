using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieApp.Models;
using Newtonsoft.Json;
using RestSharp;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public MovieSearchResult? SearchResult { get; set; }
        public List<Result> UpcomingMoviesFromTurkey { get; set; } = new List<Result>();
        public List<string> SearchedKeywords { get; set; } = new List<string>();
        public string Keyword { get; set; }
        public bool ShowSearchBar { get; set; } = true;
        public string Language { get; set; } = "tr";

        public HttpContext GetHttpContext()
        {
            return HttpContext;
        }

        public async Task<IActionResult> OnGetAsync(string keyword, string lang)
        {
            var searchedKeywords = TempData["SearchedKeywords"] as string;
            if (!string.IsNullOrEmpty(searchedKeywords))
            {
                SearchedKeywords = searchedKeywords.Split(',').ToList();
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                Keyword = keyword;
                SearchedKeywords.Insert(0, keyword);
            }

            if (SearchedKeywords.Count > 5)
            {
                SearchedKeywords.RemoveAt(SearchedKeywords.Count - 1);
            }

            if (!string.IsNullOrEmpty(lang))
            {
                Language = lang;
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                await SearchMovies(keyword);
                ShowSearchBar = false;
            }
            else
            {
                ShowSearchBar = true;
            }

            TempData["SearchedKeywords"] = string.Join(",", SearchedKeywords);

            await GetUpcomingMoviesFromTurkey();

            return Page();
        }
        private async Task SearchMovies(string keyword)
        {
            try
            {
                var options = new RestClientOptions($"https://api.themoviedb.org/3/search/movie?query={keyword}");
                var client = new RestClient(options);
                var request = new RestRequest("");
                request.AddHeader("accept", "application/json");
                var githubToken = _configuration.GetSection("Tokens:MovieDb").Value;
                request.AddHeader("Authorization", $"Bearer {githubToken}");
                var response = await client.GetAsync(request);

                var output = response.Content;
                MovieSearchResult deserializedProduct = JsonConvert.DeserializeObject<MovieSearchResult>(output);

                SearchResult = deserializedProduct;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during movie search: {ex.Message}");
                SearchResult = null;
            }
        }
        private async Task GetUpcomingMoviesFromTurkey()
        {
            try
            {
                var options = new RestClientOptions("https://api.themoviedb.org/3/discover/movie?with_original_language=tr");
                var client = new RestClient(options);
                var request = new RestRequest("");
                request.AddHeader("accept", "application/json");
                var githubToken = _configuration.GetSection("Tokens:MovieDb").Value;
                request.AddHeader("Authorization", $"Bearer {githubToken}");
                var response = await client.GetAsync(request);

                var output = response.Content;
                var deserializedMovies = JsonConvert.DeserializeObject<MovieSearchResult>(output);

                UpcomingMoviesFromTurkey = deserializedMovies.results;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching upcoming movies: {ex.Message}");
                UpcomingMoviesFromTurkey = new List<Result>();
            }
        }
    }
}