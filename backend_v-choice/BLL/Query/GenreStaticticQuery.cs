using DAL.Enum;

namespace BLL.Query
{
    public class GenreStaticticQuery : PaginationQueryBase
    {
        public GenreStatisticSortingType query { get; set; }
    }
}
