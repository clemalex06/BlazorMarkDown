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
        [Route("api/markdown")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MarkDownModel))]
        public async Task<IActionResult> Get([FromQuery] long currentId)
        {
            try
            {

                var markDownModel = new MarkDownModel
                {
                    Body = string.Empty,
                };

                var document = await markDownService.GetDocument(currentId);

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

        [HttpGet]
        [Route("api/markdowns/")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MarkDownModel>))]
        public async Task<IActionResult> GetAll()
        {
            try
            {

                var documents = await markDownService.GetAllDocuments();

                var markdowns = documents?.Select(d => new MarkDownModel
                {
                    Id = d.Id,
                    Body = d.MarkDown,
                }).ToList();



                return Ok(markdowns);
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
                var updatedDocument = await markDownService.UpdateDocument(markDownModel.Id, markDownModel.Body);

                if (!string.IsNullOrWhiteSpace(updatedDocument?.MarkDown))
                {
                    var updatedMarkDownModel = new MarkDownModel
                    {
                        Id= markDownModel.Id,
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

        [Authorize]
        [HttpPut]
        [Route("api/markdown/")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MarkDownModel))]
        public async Task<IActionResult> Put([FromBody] MarkDownModel markDownModel)
        {
            if (string.IsNullOrWhiteSpace(markDownModel?.Body))
            {
                return BadRequest("model is not OK");
            }

            try
            {
                var insertedDocument = await markDownService.InsertDocument(markDownModel.Body);

                if (!string.IsNullOrWhiteSpace(insertedDocument?.MarkDown))
                {
                    var insertedMarkDownModel = new MarkDownModel
                    {
                        Id = insertedDocument.Id,
                        Body = insertedDocument.MarkDown,
                    };
                    return Ok(insertedMarkDownModel);
                }
                else
                {
                    throw new Exception("Unable to retrieve inserted document");
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
