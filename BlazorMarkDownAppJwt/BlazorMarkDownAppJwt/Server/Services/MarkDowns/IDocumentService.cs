using BlazorMarkDownAppJwt.Server.Entities;

namespace BlazorMarkDownAppJwt.Server.Services.MarkDowns
{
    public interface IDocumentService
    {
        Task<Document?> GetMarkdown(); 
    }
}
