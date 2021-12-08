using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace inlogapp.Models
{
    public class MovieDetail
    {
        public String overview { get; set; }
        public String release_date { get; set; }
        public Genre[] genres { get; set; }
        public int id { get; set; }
        public String title { get; set; }
        public int runtime { get; set; }
        public int budget { get; set; }
        public int revenue { get; set; }
        public String backdrop_path { get; set; }
        public Double vote_average { get; set; }

        public UriImageSource backdrop_path_complete
        {
            get
            {
                return new UriImageSource
                {
                    Uri = new Uri("https://image.tmdb.org/t/p/w300_and_h450_bestv2" + backdrop_path),
                    CachingEnabled = true,
                    CacheValidity = new TimeSpan(3, 0, 0, 0)
                };
            }
        }
    }
}
