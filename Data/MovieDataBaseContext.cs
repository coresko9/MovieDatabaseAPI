using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieDataBase.Models;

namespace MovieDataBase.Data
{
    public class MovieDataBaseContext : DbContext
    {
        public MovieDataBaseContext (DbContextOptions<MovieDataBaseContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movie { get; set; }
       
    }
    
}
