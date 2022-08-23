using BlazorMarkDownAppJwt.Server.Entities;

namespace BlazorMarkDownAppJwt.Server.Services.MarkDowns
{
    public class MarkDownService : IMarkDownService
    {

        private readonly IWebHostEnvironment env;

        private const string markDownPath = "Datas\\Json\\Markdown";
        public MarkDownService(IWebHostEnvironment env) => this.env = env;
        public async Task<MarkDown?> GetMarkdown()
        {
            var path = Path.Combine(env.ContentRootPath, markDownPath);
            if (!Directory.Exists(path))
                return null;
            path = Path.Combine(path, "test.md");
            if (!File.Exists(path))
                return null;
            return new MarkDown
            {
                Id = 0,
                Document = await File.ReadAllTextAsync(path),
            };
        }
    }
}
