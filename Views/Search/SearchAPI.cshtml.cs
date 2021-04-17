using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieDataBase.Data;
using MovieDataBase.Models;

namespace MovieDataBase.Views.Search
{
    public class SearchAPIModel : PageModel
    {



        private readonly MovieDataBaseContext _context;
        public SearchAPIModel(MovieDataBaseContext context)
        {
            _context = context;
           
        }
        public void OnGet()
        {
        }
        public IActionResult OnPostAddMovie([Bind("Id,Title,ReleaseDate")] Movie movie)
        {
            _context.Add(new Movie()
            {
                Id= 10,
                Title = movie.Title,
                ReleaseDate = movie.ReleaseDate
            }) ;
            return RedirectToAction("Index", "Movies");
        }

    }
}
