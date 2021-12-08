using System;
using System.Collections.Generic;
using System.Text;

namespace inlogapp.Models
{
    public class QueryResult
    {
        public int page { get; set; }

        public Movie[] results { get; set; }

        public MovieDate dates { get; set; }
    }
}
