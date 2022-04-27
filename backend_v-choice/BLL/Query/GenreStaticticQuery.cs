using DAL.Enum;

namespace BLL.Query
{
    public class GenreStaticticQuery : PaginationQueryBase
    {
        public GenreStatisticSortingType SortingType { get; set; }
    }
}
