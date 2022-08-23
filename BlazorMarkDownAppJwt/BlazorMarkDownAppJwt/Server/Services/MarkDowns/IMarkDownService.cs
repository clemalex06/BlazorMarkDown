using BlazorMarkDownAppJwt.Server.Entities;

namespace BlazorMarkDownAppJwt.Server.Services.MarkDowns
{
    public interface IMarkDownService
    {
        Task<MarkDown?> GetMarkdown(); 
    }
}
