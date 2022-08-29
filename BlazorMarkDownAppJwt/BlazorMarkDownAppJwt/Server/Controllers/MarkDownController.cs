using BlazorMarkDownAppJwt.Server.Services.MarkDowns;
using BlazorMarkDownAppJwt.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Net;

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
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }

        }

        [HttpGet]
        [Route("api/markdown/readme/")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MarkDownModel))]
        public async Task<IActionResult> GetReadMe()
        {
            try
            {

                var markDownModel = new MarkDownModel
                {
                    Body = string.Empty,
                };

                var document = await markDownService.GetReadMeDocument();

                if (document?.MarkDown != null)
                {

                    markDownModel.Body = document.MarkDown;
                }

                return Ok(markDownModel);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
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
                    throw new Exception("Unable to retrieve updated document");
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
