using BLL.Enum;

namespace BLL.Query
{
    public class PaginationQueryFilms : PaginationQueryBase
    {
        public int? GenreId { get; set; }
        public SortingType? SortBy { get; set; }
        public bool? HasCommentsOnly { get; set; }
        public bool? HasRateOnly { get; set; }
        public int? StudioId { get; set; }
        public int? DirectorId { get; set; }
        public int? ActorId { get; set; }
        public string Search { get; set; }
        public int? YearMin { get; set; }
        public int? YearMax { get; set; }
        public float? RateMin { get; set; }
        public float? RateMax { get; set; }
    }
}
