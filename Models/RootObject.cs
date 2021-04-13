using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDataBase.Models
{
    public partial class RootObject
    {
        public MoviesSearch[] Search { get; set; }
        public int totalResults { get; set; }
        public string Response { get; set; }
        
        
    }
}