using BlazorMarkDownAppJwt.Server.Entities;

namespace BlazorMarkDownAppJwt.Server.Services.MarkDowns
{
    public interface IDocumentService
    {
        Task<Document?> GetDocument();

        Task<List<Document>> GetAllDocuments();

        Task<Document?> UpsertDocument(string markDown);

        Task<Document?> GetReadMeDocument();
    }
}
