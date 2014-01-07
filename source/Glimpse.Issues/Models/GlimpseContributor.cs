namespace Glimpse.Infrastructure.Models
{
    public class GlimpseContributor
    {
        public string Name { get; set; }
        public string GithubUsername { get; set; }
        public string TwitterUsername { get; set; }
        public string Category { get; set; }
        public int TotalContributions { get; set; }
    }
}