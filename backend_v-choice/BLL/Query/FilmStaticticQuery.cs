using DAL.Enum;

namespace BLL.Query
{
    public class FilmStaticticQuery : PaginationQueryBase
    {
        public FilmStatisticSortingType query { get; set; }
    }
}
