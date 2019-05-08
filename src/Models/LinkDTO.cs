namespace WhiskyApp.Models
{
    public class LinkDTO
    {
        public string Href { get; private set; }
        public string Rel { get; private set; }
        public string Method { get; private set; }

        public LinkDTO(string href, string rel, string method)
        {
            this.Href = href;
            this.Rel = rel;
            this.Method = method;
        }
    }
}