using BlazorMarkDownAppJwt.Server.Entities;

namespace BlazorMarkDownAppJwt.Server.Services.MarkDowns
{
    public interface IDocumentService
    {
        Task<Document?> GetDocument();
        Task<Document?> UpsertDocument(string markDown);
    }
}
