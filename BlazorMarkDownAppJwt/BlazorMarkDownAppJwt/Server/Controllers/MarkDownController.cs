using BlazorMarkDownAppJwt.Server.Services.MarkDowns;
using BlazorMarkDownAppJwt.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BlazorMarkDownAppJwt.Server.Entities;

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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MarkDownModel))]
        public async Task<IActionResult> Get()
        {
            try
            {

                var markDownModel = new MarkDownModel
                {
                    Body = string.Empty,
                };

                var document = await markDownService.GetDocument();

                if (document?.MarkDown != null)
                {

                    markDownModel.Body = document.MarkDown;
                }

                return Ok(markDownModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [Authorize]
        [HttpPost]
        [Route("api/markdown/")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MarkDownModel))]
        public async Task<IActionResult> Post([FromBody] MarkDownModel markDownModel)
        {
            if (string.IsNullOrWhiteSpace(markDownModel?.Body))
            {
                return BadRequest("model is not OK");
            }

            try
            {
                var updatedDocument = await markDownService.UpsertDocument(markDownModel.Body);

                if (updatedDocument?.MarkDown != null)
                {
                    var updatedMarkDownModel = new MarkDownModel
                    {
                        Body = updatedDocument.MarkDown,
                    };
                    return Ok(updatedMarkDownModel);
                }
                else
                {
                    return BadRequest("Unable to retrieve updated document");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
