using Microsoft.AspNetCore.Mvc;
using MovieDataBase.Models;
using Newtonsoft.Json;
using RestSharp;

namespace MovieDataBase.Controllers
{
    public class SearchController : Controller
    {
        private IRestResponse _resp;
        private string search;


        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SearchAPI(string movieTitle)
        {
            if(!string.IsNullOrWhiteSpace(movieTitle))
            {
                movieTitle.Trim();
            }
            else
            {
                movieTitle = "*";
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
           if (object.Equals(null,movieReturn.Search))
                return RedirectPermanent($"/Search/NoResults");
            else
                return View(movieReturn);
        }
        public IActionResult NoResults()
        {
            return View();
        }
    }
}
