using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace inlogapp.Models
{
    public class Movie
    {
        public String poster_path { get; set; }
        public Boolean adult { get; set; }
        public String overview { get; set; }
        public String release_date { get; set; }
        public Int16[] genre_ids { get; set; }
        public int id { get; set; }
        public String original_title { get; set; }
        public String original_language { get; set; }
        public String title { get; set; }
        public String backdrop_path { get; set; }
        public Double popularity { get; set; }
        public int vote_count { get; set; }
        public Boolean video { get; set; }
        public Double vote_average { get; set; }

        public UriImageSource poster_path_complete
        {
            get
            { 
                return new UriImageSource
                {
                    Uri = new Uri("https://image.tmdb.org/t/p/w300_and_h450_bestv2" + poster_path),
                    CachingEnabled = true,
                    CacheValidity = new TimeSpan(3,0,0,0)
                };
            }
        }

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
