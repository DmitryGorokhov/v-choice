namespace BLL.DTO
{
    public class FilmStatisticDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Requested { get; set; }
        public float AvRate { get; set; }
        public int CountRate { get; set; }
        public int CountComment { get; set; }
        public int CountFavorite { get; set; }
    }
}
