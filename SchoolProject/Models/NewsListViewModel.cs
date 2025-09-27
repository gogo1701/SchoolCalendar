namespace SchoolProject.Models
{
    public class NewsListViewModel
    {
        public IEnumerable<News> News { get; set; } = Enumerable.Empty<News>();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }

}
