using BlazorMarkDownAppJwt.Server.Services.MarkDowns;
using BlazorMarkDownAppJwt.Shared;
using Microsoft.AspNetCore.Mvc;

namespace BlazorMarkDownAppJwt.Server.Controllers
{
    [ApiController]
    public class MarkDownController : Controller
    {
        private IDocumentService markDownService { get; }

        public MarkDownController(IDocumentService markDownService)
        {
            this.markDownService = markDownService;
        }

        [HttpGet]
        [Route("api/markdown/")]
        public async Task<MarkDownModel> Get()
        {
            string markdownDocument = string.Empty;
            var document = await markDownService.GetMarkdown();

            if (document?.MarkDown != null)
                markdownDocument = document.MarkDown;

            return new MarkDownModel
            {
                Body = markdownDocument,
        };
        }
    }
}
