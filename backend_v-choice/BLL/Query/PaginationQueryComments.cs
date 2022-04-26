namespace BLL.Query
{
    public class PaginationQueryComments : PaginationQueryBase
    {
        public int FilmId { get; set; }
        public bool CommonOrder { get; set; }
        public bool MyCommentsFirst { get; set; }
    }
}
