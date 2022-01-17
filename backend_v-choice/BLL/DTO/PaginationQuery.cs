using System.ComponentModel.DataAnnotations;

namespace BLL.DTO
{
    public class PaginationQuery
    {
        [Required]
        public int PageNumber { get; set; }

        [Required]
        public int OnPageCount { get; set; }

        public int? GenreId { get; set; }

        public int? FilmId { get; set; }

        public int? SortBy { get; set; }

        public bool? CommonOrder { get; set; }

        public bool? MyCommentsFirst { get; set; }

        public bool? HasCommentsOnly { get; set; }

        public bool? WithoutMyRateOnly { get; set; }
    }
}
