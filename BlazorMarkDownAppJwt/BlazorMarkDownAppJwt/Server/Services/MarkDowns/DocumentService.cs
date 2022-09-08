using BlazorMarkDownAppJwt.Server.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorMarkDownAppJwt.Server.Services.MarkDowns
{
    public class DocumentService : IDocumentService
    {

        private readonly DataBaseContext ctx;

        private readonly IWebHostEnvironment env;

        private const string documentPath = "Datas";


        public DocumentService(DataBaseContext ctx, IWebHostEnvironment env)
        {
            this.ctx = ctx;
            this.env = env;
        }
        public async Task<Document?> GetDocumentAsync(long id, CancellationToken cancellationToken)
        {
            var document = await ctx.Document.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
            return document;
        }

        public async Task<Document> InsertDocumentAsync(string markDown, CancellationToken cancellationToken)
        {
            Document document = new Document
            {
                MarkDown = markDown
            };
            ctx.Add(document);

            await ctx.SaveChangesAsync(cancellationToken);

            return document;
        }

        public async Task<Document?> GetReadMeDocumentAsync(CancellationToken cancellationToken)
        {
            var path = Path.Combine(env.ContentRootPath, documentPath);
            if (!Directory.Exists(path))
                return null;
            path = Path.Combine(path, "README.md");
            if (!File.Exists(path))
                return null;
            return new Document
            {
                Id = 0,
                MarkDown = await File.ReadAllTextAsync(path, cancellationToken),
            };
        }

        public async Task<List<Document>> GetAllDocumentsAsync(CancellationToken cancellationToken)
        {
            var result = await ctx.Document.ToListAsync(cancellationToken);
            return result;
        }

        public async Task<Document> UpdateDocumentAsync(long id, string markDown, CancellationToken cancellationToken)
        {
            var document = await ctx.Document.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
            if (document != null)
            {
                document.MarkDown = markDown;
                await ctx.SaveChangesAsync(cancellationToken);
                return document;
            }
            throw new Exception($"unable to find document with id = {id}");

        }
    }
}
