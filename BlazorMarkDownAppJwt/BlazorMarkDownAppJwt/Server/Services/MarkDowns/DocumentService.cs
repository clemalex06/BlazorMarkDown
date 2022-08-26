using BlazorMarkDownAppJwt.Server.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorMarkDownAppJwt.Server.Services.MarkDowns
{
    public class DocumentService : IDocumentService
    {

        private DataBaseContext Ctx { get; set; }

        public DocumentService(DataBaseContext ctx)
        {
            Ctx = ctx;
        }
        public async Task<Document?> GetDocument()
        {
            var document = await Ctx.Document.SingleOrDefaultAsync();
            return document;
        }

        public async Task<Document?> UpsertDocument(string markDown)
        {
            var document = await Ctx.Document.SingleOrDefaultAsync();

            if (document == null)
            {
                document = new Document
                {
                    MarkDown = markDown
                };
                Ctx.Add(document);
            }
            else
            {
                document.MarkDown = markDown;
            }

            await Ctx.SaveChangesAsync();

            return document;
        }
    }
}
