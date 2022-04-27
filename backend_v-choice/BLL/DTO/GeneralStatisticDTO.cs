namespace BLL.DTO
{
    public class GeneralStatisticDTO
    {
        public int FilmsTotal { get; set; }
        public int FilmsRated { get; set; }
        public int FilmsNotRated { get; set; }
        public int FilmsCommented { get; set; }
        public int FilmsNotCommented { get; set; }
        public int MinYear { get; set; }
        public int MaxYear { get; set; }
        public int CommentsTotal { get; set; }
        public int CommentsMax { get; set; }
    }
}
