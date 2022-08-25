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
        public async Task<Document?> GetMarkdown()
        {
            var markDown = await Ctx.Document.SingleOrDefaultAsync();
            return markDown;
        }
    }
}
