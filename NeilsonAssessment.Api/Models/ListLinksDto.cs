namespace NeilsonAssessment.Api.Models
{
    public class ListLinksDto
    {
        public LinkDto Self { get; set; }
        public LinkDto First { get; set; }
        public LinkDto Prev { get; set; }
        public LinkDto Next { get; set; }
        public LinkDto Last { get; set; }
    }
}
