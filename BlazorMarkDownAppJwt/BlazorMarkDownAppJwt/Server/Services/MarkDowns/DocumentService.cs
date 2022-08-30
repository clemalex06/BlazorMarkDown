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
        public async Task<Document?> GetDocument(long id)
        {
            var document = await ctx.Document.FirstOrDefaultAsync(p => p.Id == id);
            return document;
        }

        public async Task<Document?> InsertDocument(string markDown)
        {
            var document = new Document
            {
                MarkDown = markDown
            };
            ctx.Add(document);

            await ctx.SaveChangesAsync();

            return document;
        }

        public async Task<Document?> GetReadMeDocument()
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
                MarkDown = await File.ReadAllTextAsync(path),
            };
        }

        public async Task<List<Document>> GetAllDocuments()
        {
            return await ctx.Document.ToListAsync();
        }

		public async Task<Document?> UpdateDocument(long id, string markDown)
		{
            var document = await ctx.Document.FirstOrDefaultAsync(p => p.Id == id);
            if (document != null)
            {
                document.MarkDown = markDown;
                await ctx.SaveChangesAsync();
            }
            return document;
        }
	}
}
