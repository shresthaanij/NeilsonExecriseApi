namespace NeilsonAssessment.Api.Models
{
    public class LinkDto
    {
        public string Href { get; set; }
        public string REL { get; set; }
        public string Method { get; set; }

        public LinkDto(string href, string rel, string method)
        {
            Href = href;
            REL = rel;
            Method = method;
        }
    }
}
