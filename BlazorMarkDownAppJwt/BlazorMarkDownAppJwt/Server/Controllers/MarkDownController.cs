using BlazorMarkDownAppJwt.Server.Services.MarkDowns;
using BlazorMarkDownAppJwt.Shared;
using Markdig;
using Microsoft.AspNetCore.Mvc;

namespace BlazorMarkDownAppJwt.Server.Controllers
{
    [ApiController]
    public class MarkDownController : Controller
    {
        private IMarkDownService markDownService { get; }

        public MarkDownController(IMarkDownService markDownService)
        {
            this.markDownService = markDownService;
        }

        [HttpGet]
        [Route("api/markdown/")]
        public async Task<MarkDownModel> Get()
        {
            string markdownDocument = string.Empty;
            var markdown = await markDownService.GetMarkdown();

            if (markdown?.Document != null)
                markdownDocument = markdown.Document;

            return new MarkDownModel
            {
                Body = markdownDocument,
                Preview = Markdown.ToHtml(markdownDocument),
        };
        }
    }
}
