using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDataBase.Models
{
    public class MovieListViewModel
    {

        public IEnumerable<Movie> Movies { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string ViewString { get; set; }
    }
}
