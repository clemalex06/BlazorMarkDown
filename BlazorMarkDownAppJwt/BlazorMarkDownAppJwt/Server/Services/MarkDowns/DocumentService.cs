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

            // TODO implement business code to update the document

            return document;
        }
    }
}
