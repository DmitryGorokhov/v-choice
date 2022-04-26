using DAL.Model;

namespace BLL.Query
{
    public class PaginationQueryFilms : PaginationQueryBase
    {
        public int? GenreId { get; set; }
        public SortingType? SortBy { get; set; }
        public bool? HasCommentsOnly { get; set; }
        public bool? HasRateOnly { get; set; }
    }
}
