using BlazorMarkDownAppJwt.Server.Services.MarkDowns;
using BlazorMarkDownAppJwt.Shared;
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
            var markdown = await markDownService.GetMarkdown();

            return new MarkDownModel
            {
                Body = markdown != null ? markdown.Document : String.Empty,
        };
        }
    }
}
