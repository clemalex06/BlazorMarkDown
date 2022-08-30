using BlazorMarkDownAppJwt.Server.Entities;

namespace BlazorMarkDownAppJwt.Server.Services.MarkDowns
{
    public interface IDocumentService
    {
        Task<Document?> GetDocument(long id);

        Task<List<Document>> GetAllDocuments();

        Task<Document?> InsertDocument(string markDown);

        Task<Document?> UpdateDocument(long id, string markDown);

        Task<Document?> GetReadMeDocument();
    }
}
