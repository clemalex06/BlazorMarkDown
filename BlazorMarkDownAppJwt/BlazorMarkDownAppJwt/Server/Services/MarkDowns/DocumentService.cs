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
            var markDown = await Ctx.Document.SingleOrDefaultAsync();
            return markDown;
        }

        public Task<Document?> UpsertDocument(string markDown)
        {
            throw new NotImplementedException();
        }
    }
}
