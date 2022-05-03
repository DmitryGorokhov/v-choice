using DAL.Enum;

namespace BLL.Query
{
    public class ExportStatisticQuery
    {
        public FilmStatisticSortingType FilmSortingType { get; set; }
        public GenreStatisticSortingType GenreSortingType { get; set; }
    }
}
