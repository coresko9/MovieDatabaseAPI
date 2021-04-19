using Microsoft.AspNetCore.Mvc;
using MovieDataBase.Data;
using MovieDataBase.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace MovieDataBase.Controllers
{
    
    public class SearchController : Controller
    {
        
        private IRestResponse _resp;
        private string search;

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
            //new
            ViewModelMovie data = new ViewModelMovie();

            if(!string.IsNullOrWhiteSpace(movieTitle))
            {
                movieTitle = movieTitle.Trim();
            }
            else
            {
                movieTitle = " ";
            }
            var client = new RestClient("https://movie-database-imdb-alternative.p.rapidapi.com/").AddDefaultQueryParameter("s", movieTitle);
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-key", "08d9fc5c80mshe30902b3b9069d6p1f9f04jsnd7eaa1d2d693");
            request.AddHeader("x-rapidapi-host", "movie-database-imdb-alternative.p.rapidapi.com");
            IRestResponse response = client.Execute(request);
            var movieReturn = new RootObject();

            this._resp = response;
            movieReturn = JsonConvert.DeserializeObject<RootObject>(this._resp.Content);
            this.search = movieTitle;
            //new
            data.SearchResults = movieReturn;
            if (object.Equals(null, movieReturn.Search))
            {
                ViewBag.SearchString = $"No results for \"{movieTitle}\"";

                ViewBag.noResults = true;
                //changed from movieReturn
                return View(data);

            }
            else
            {
                ViewBag.SearchString = $"Results for \"{movieTitle}\"";
                ViewBag.noResults = false;
                //changed from movieReturn
                return View(data);
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddMovie([Bind("Id,Title,ReleaseDate")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                
            }
            return View("SearchAPI",search);

        }

    }
    
}
