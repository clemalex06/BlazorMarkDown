using Markdig;


namespace BlazorMarkDownAppJwt.Shared
{
    public class MarkDownModel
    {
        public long Id { get; set; }

        public string Body { get; set; }

        public string Html { get { return Markdown.ToHtml(Body); } }
    }
}
