using BlazorMarkDownAppJwt.Server.Entities;

namespace BlazorMarkDownAppJwt.Server.Services.MarkDowns
{
    public class DocumentJsonService : IDocumentService
    {

        private readonly IWebHostEnvironment env;

        private const string documentPath = "Datas\\Json\\Markdown";
        public DocumentJsonService(IWebHostEnvironment env) => this.env = env;
        public async Task<Document?> GetDocument()
        {
            var path = Path.Combine(env.ContentRootPath, documentPath);
            if (!Directory.Exists(path))
                return null;
            path = Path.Combine(path, "test.md");
            if (!File.Exists(path))
                return null;
            return new Document
            {
                Id = 0,
                MarkDown = await File.ReadAllTextAsync(path),
            };
        }

        public Task<Document?> UpsertDocument(string markDown)
        {
            throw new NotImplementedException();
        }
    }
}
