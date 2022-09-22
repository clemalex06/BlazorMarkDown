using BlazorMarkDownAppJwt.Server.Entities;

namespace BlazorMarkDownAppJwt.Server.Services.MarkDowns
{
    public interface IDocumentService
    {
        Task<Document?> GetDocumentAsync(long id, CancellationToken cancellationToken);

        Task<List<Document>> GetAllDocumentsAsync(CancellationToken cancellationToken);

        Task<Document> InsertDocumentAsync(string markDown, CancellationToken cancellationToken);

        Task<Document> UpdateDocumentAsync(long id, string markDown, CancellationToken cancellationToken);

        Task<Document?> GetReadMeDocumentAsync(CancellationToken cancellationToken);
    }
}
