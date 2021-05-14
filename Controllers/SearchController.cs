using Microsoft.AspNetCore.Mvc;
using MovieDataBase.Data;
using MovieDataBase.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MovieDataBase.Controllers
{

    public class SearchController : Controller
    {
        private IRestResponse _resp;
        private static string _search;
        private static ViewModelMovie _data;

        private readonly MovieDataBaseContext _context;
        public SearchController(MovieDataBaseContext context) 
        { 
            _context = context;



        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SearchAPI(string movieTitle)
        {

            _data = new ViewModelMovie();

            if (!string.IsNullOrWhiteSpace(movieTitle))
            {
                movieTitle = movieTitle.Trim();
            }
            else
            {
                movieTitle = " ";
            }


            var client = new RestClient("https://movie-database-imdb-alternative.p.rapidapi.com/").AddDefaultQueryParameter("s", movieTitle);
            var request = new RestRequest(Method.GET);
            //ADD API KEY AND HEADER HERE
            request.AddHeader("x-rapidapi-key", "08d9fc5c80mshe30902b3b9069d6p1f9f04jsnd7eaa1d2d693");
            request.AddHeader("x-rapidapi-host", "movie-database-imdb-alternative.p.rapidapi.com");
            IRestResponse response = client.Execute(request);
            var movieReturn = new RootObject();

            this._resp = response;
            movieReturn = JsonConvert.DeserializeObject<RootObject>(this._resp.Content);
            _search = movieTitle;
            //new
            _data.SearchResults = movieReturn;

            if (object.Equals(null, movieReturn.Search))
            {
                ViewBag.SearchString = $"No results for \"{movieTitle}\"";

                ViewBag.noResults = true;
                //changed from movieReturn
                return View(_data);
            }
            else
            {

                ViewBag.SearchString = $"Results for \"{movieTitle}\"";
                ViewBag.noResults = false;
                //changed from movieReturn
                return View(_data);
            }
        }
        [HttpPost]
        public IActionResult Discover(string genre)
        {
            RootDiscovery rootDiscovery = new RootDiscovery();
            var client = new RestClient($"https://imdb8.p.rapidapi.com/title/get-popular-movies-by-genre?genre=%2Fchart%2Fpopular%2Fgenre%2F{genre.ToLower()}");
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-key", "08d9fc5c80mshe30902b3b9069d6p1f9f04jsnd7eaa1d2d693");
            request.AddHeader("x-rapidapi-host", "imdb8.p.rapidapi.com");
            IRestResponse response = client.Execute(request);
            string[] discoverTitleRaw = JsonConvert.DeserializeObject<string[]>(response.Content);
            List<string> discoverTitleOnly = new List<string>();
            foreach (string rawTitle in discoverTitleRaw)
            {
                discoverTitleOnly.Add(rawTitle.Substring(rawTitle.IndexOf("tt")).Replace('/', ' ').Trim());
            }
            rootDiscovery.TopByGenre = discoverTitleOnly.ToArray();
            _data = new ViewModelMovie();
            int countReturn = 0;
            List<RandomMovie> movieList = new List<RandomMovie>();
            for (int i = 0; countReturn < 50 && i <rootDiscovery.TopByGenre.Length; i++, countReturn++)
            {
                var newClient = new RestClient("https://movie-database-imdb-alternative.p.rapidapi.com/").AddDefaultQueryParameter("i", rootDiscovery.TopByGenre[i]);
                request = new RestRequest(Method.GET);

                //add api Key and header Here
                request.AddHeader("x-rapidapi-key", "08d9fc5c80mshe30902b3b9069d6p1f9f04jsnd7eaa1d2d693");
                request.AddHeader("x-rapidapi-host", "movie-database-imdb-alternative.p.rapidapi.com");

                response = newClient.Execute(request);
                movieList.Add(JsonConvert.DeserializeObject<RandomMovie>(response.Content));
            }
            _data.RandomMovies = movieList;
        
            return View(_data);

        }       
        public IActionResult Random()
        {
           
            
            Random ranNum = new Random();
            int masterCounter = 0;
            List<RandomMovie> movieReturnList = new List<RandomMovie>();
            RandomMovie randomMovie = new RandomMovie();
            _data = new ViewModelMovie();
            while (movieReturnList.Count < 5 && masterCounter <25)
            {
                randomMovie = null;
                var client = new RestClient("https://movie-database-imdb-alternative.p.rapidapi.com/").AddDefaultQueryParameter("i", $"tt{ranNum.Next(10000001, 19916778).ToString().Substring(1)}");
                var request = new RestRequest(Method.GET);

                //add api Key and header Here
                request.AddHeader("x-rapidapi-key", "08d9fc5c80mshe30902b3b9069d6p1f9f04jsnd7eaa1d2d693");
                request.AddHeader("x-rapidapi-host", "movie-database-imdb-alternative.p.rapidapi.com");

                IRestResponse response = client.Execute(request);
                
                randomMovie = JsonConvert.DeserializeObject<RandomMovie>(response.Content);
              


                if (!string.IsNullOrWhiteSpace(randomMovie.Title) || !string.IsNullOrWhiteSpace(randomMovie.Poster) )
                {
                    movieReturnList.Add(randomMovie);
                }
                masterCounter++;
            }
            _data.RandomMovies = movieReturnList;
            return View("Random",_data);
        }
        public IActionResult AddMovie(string imdbID,string Title)
        {
            ViewBag.IsAdded = true;
            if (_context.Movie.Any(x => x.imdbID == imdbID))
            {
                ViewBag.OnAdded = $"\"{Title}\" already exists";
                ViewBag.SearchString = $"Results for \"{_search}\"";
                ViewBag.noResults = false;
                ViewBag.IsAdded = false;
                return View("SearchAPI", _data);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    Movie detailedMovie = new Movie();
                    var client = new RestClient("https://movie-database-imdb-alternative.p.rapidapi.com/").AddDefaultQueryParameter("i", $"{imdbID}");
                    var request = new RestRequest(Method.GET);
                    //ADD API KEY AND HEADER HERE
                    request.AddHeader("x-rapidapi-key", "08d9fc5c80mshe30902b3b9069d6p1f9f04jsnd7eaa1d2d693");
                    request.AddHeader("x-rapidapi-host", "movie-database-imdb-alternative.p.rapidapi.com");
                    IRestResponse response = client.Execute(request);
                    this._resp = response;

                    detailedMovie = JsonConvert.DeserializeObject<Movie>(this._resp.Content);
                    _context.Add(detailedMovie);
                    _context.SaveChanges();
                }
                ViewBag.OnAdded = $"Added: \"{Title}\"";
                ViewBag.SearchString = $"Results for \"{_search}\"";
                ViewBag.noResults = false;
                //changed from movieReturn
                return View("SearchAPI", _data);
            }
           
        }
        public IActionResult AddRandomMovie(string imdbID, string Title)
        {
            ViewBag.IsAdded = true;
            if (_context.Movie.Any(x => x.imdbID == imdbID))
            {
                ViewBag.OnAdded = $"\"{Title}\" already exists";
                ViewBag.SearchString = $"Results for \"{_search}\"";
                ViewBag.noResults = false;
                ViewBag.IsAdded = false;
                return View("Random", _data);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    Movie detailedMovie = new Movie();
                    var client = new RestClient("https://movie-database-imdb-alternative.p.rapidapi.com/").AddDefaultQueryParameter("i", $"{imdbID}");
                    var request = new RestRequest(Method.GET);
                    //ADD API KEY AND HEADER HERE
                    request.AddHeader("x-rapidapi-key", "08d9fc5c80mshe30902b3b9069d6p1f9f04jsnd7eaa1d2d693");
                    request.AddHeader("x-rapidapi-host", "movie-database-imdb-alternative.p.rapidapi.com");
                    IRestResponse response = client.Execute(request);
                    this._resp = response;

                    detailedMovie = JsonConvert.DeserializeObject<Movie>(this._resp.Content);
                    _context.Add(detailedMovie);
                    _context.SaveChanges();
                    ViewBag.OnAdded = $"Added: \"{Title}\"";
                    ViewBag.SearchString = $"Results for \"{_search}\"";
                    ViewBag.noResults = false;
                }

                //changed from movieReturn
                return View("Random", _data);
            }

        }
        public IActionResult AddDiscoverMovie(string imdbID, string Title)
        {
            ViewBag.IsAdded = true;
            if (_context.Movie.Any(x => x.imdbID == imdbID))
            {
                ViewBag.OnAdded = $"\"{Title}\" already exists";
                ViewBag.SearchString = $"Results for \"{_search}\"";
                ViewBag.noResults = false;
                ViewBag.IsAdded = false;
                return View("Discover", _data);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    Movie detailedMovie = new Movie();
                    var client = new RestClient("https://movie-database-imdb-alternative.p.rapidapi.com/").AddDefaultQueryParameter("i", $"{imdbID}");
                    var request = new RestRequest(Method.GET);
                    //ADD API KEY AND HEADER HERE
                    request.AddHeader("x-rapidapi-key", "08d9fc5c80mshe30902b3b9069d6p1f9f04jsnd7eaa1d2d693");
                    request.AddHeader("x-rapidapi-host", "movie-database-imdb-alternative.p.rapidapi.com");
                    IRestResponse response = client.Execute(request);
                    this._resp = response;

                    detailedMovie = JsonConvert.DeserializeObject<Movie>(this._resp.Content);
                    _context.Add(detailedMovie);
                    _context.SaveChanges();
                    ViewBag.OnAdded = $"Added: \"{Title}\"";
                    ViewBag.SearchString = $"Results for \"{_search}\"";
                    ViewBag.noResults = false;
                }

                //changed from movieReturn
                return View("Discover", _data);
            }

        }


        [HttpGet]
        public IActionResult Discover()
        {
            return View();
        }
        
    }

}
