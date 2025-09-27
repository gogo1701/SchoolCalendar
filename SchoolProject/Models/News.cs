namespace SchoolProject.Models
{
    public class News
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string ImageURL { get; set; }
        public DateTime DatePublished { get; set; }
        public DateTime? DateOnCalendar { get; set; }
    }
}
