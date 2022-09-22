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
        private IDocumentService MarkDownService { get; }

        public MarkDownController(IDocumentService markDownService)
        {
            this.MarkDownService = markDownService;
        }

        [HttpGet]
        [Route("api/markdown")]
        public async Task<ActionResult<MarkDownModel>> GetAsync([FromQuery] long currentId, CancellationToken cancellationToken)
        {
            try
            {

                var markDownModel = new MarkDownModel
                {
                    Body = string.Empty,
                };

                var document = await MarkDownService.GetDocumentAsync(currentId, cancellationToken);

                if (document == null)
                {
                    return NotFound();
                }

                markDownModel.Body = document.MarkDown;
                markDownModel.Id = document.Id;

                return Ok(markDownModel);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }

        }

        [HttpGet]
        [Route("api/markdown/readme/")]
        public async Task<ActionResult<MarkDownModel>> GetReadMeAsync(CancellationToken cancellationToken)
        {
            try
            {

                var markDownModel = new MarkDownModel
                {
                    Body = string.Empty,
                };

                var document = await MarkDownService.GetReadMeDocumentAsync(cancellationToken);

                if (document == null)
                {
                    return NotFound();
                }

                markDownModel.Body = document.MarkDown;

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
        public async Task<ActionResult<List<MarkDownModel>>> GetAllAsync(CancellationToken cancellationToken)
        {
            try
            {
                var documents = await MarkDownService.GetAllDocumentsAsync(cancellationToken);

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
        [HttpPut]
        [Route("api/markdown/")]
        public async Task<ActionResult<MarkDownModel>> PutAsync([FromBody] MarkDownModel markDownModel, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(markDownModel?.Body))
            {
                return BadRequest("model is not OK");
            }

            try
            {

                var updatedDocument = await MarkDownService.UpdateDocumentAsync(markDownModel.Id, markDownModel.Body, cancellationToken);

                var updatedMarkDownModel = new MarkDownModel
                {
                    Id = updatedDocument.Id,
                    Body = updatedDocument.MarkDown,
                };
                return Ok(updatedMarkDownModel);
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
        public async Task<ActionResult<MarkDownModel>> PostAsync([FromBody] MarkDownModel markDownModel, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(markDownModel?.Body))
            {
                return BadRequest("model is not OK");
            }

            try
            {
                var insertedDocument = await MarkDownService.InsertDocumentAsync(markDownModel.Body, cancellationToken);

                var insertedMarkDownModel = new MarkDownModel
                {
                    Id = insertedDocument.Id,
                    Body = insertedDocument.MarkDown,
                };
                return Ok(insertedMarkDownModel);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
