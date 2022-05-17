using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace BLL.DTO
{
    public class FilmDTO
    {
        public FilmDTO() { }

        public int? Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string Description { get; set; }
        public int? TotalRate { get; set; }
        public int? CountRate { get; set; }
        public float? AverageRate { get; set; }
        public string PosterPath { get; set; }
        public IFormFile Poster { get; set; }
        public string VideoToken { get; set; }
        public StudioDTO Studio { get; set; }

        public ICollection<GenreDTO> Genres { get; set; }
        public ICollection<ParticipationDTO> Directors { get; set; }
        public ICollection<ParticipationDTO> Cast { get; set; }
    }
}
