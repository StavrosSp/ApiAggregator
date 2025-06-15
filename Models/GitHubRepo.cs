namespace ApiAggregator.Models
{
    public class GitHubRepo
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string Url { get; set; } = "";
        public int Stars { get; set; }
    }

}
