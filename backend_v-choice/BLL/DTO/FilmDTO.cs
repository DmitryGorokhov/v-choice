using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.DTO
{
    public class FilmDTO
    {
        public FilmDTO() { }

        public int? Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string Description { get; set; }
        public float? AverageRate { get; set; }
        public string PosterPath { get; set; }
        public IFormFile Poster { get; set; }

        public ICollection<GenreDTO> Genres { get; set; }
    }
}
