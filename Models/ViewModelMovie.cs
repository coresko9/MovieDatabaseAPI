using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDataBase.Models
{
    public class ViewModelMovie
    {
        public RootObject SearchResults { get; set; }
        public Movie Movies { get; set; }
    }
}
