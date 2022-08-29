using BlazorMarkDownAppJwt.Server.Controllers;
using BlazorMarkDownAppJwt.Server.Entities;
using BlazorMarkDownAppJwt.Server.Services.MarkDowns;
using BlazorMarkDownAppJwt.Shared;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace BlazorMarkDownAppJwt.UnitTests.Server.Controllers
{
    public class MarkDownControllerTest
    {
        private Mock<IDocumentService> DocumentServiceMoq { get; set; }

        private MarkDownController Controller { get; set; }

        [SetUp]
        public void Setup()
        {
            DocumentServiceMoq = new Mock<IDocumentService>();
            Controller = new MarkDownController(DocumentServiceMoq.Object);
        }

        [Test]
        public async Task MarkDownController_Get_ServiceReturnsDocument()
        {
            // Arrange
            var document = new Document
            {
                Id = 1,
                MarkDown = "# Hello World"
            };
            DocumentServiceMoq.Setup(d => d.GetDocument()).ReturnsAsync(document);

            // Act
            var actionResult = await Controller.Get();

            // Assert
            var okObjectResult = actionResult as OkObjectResult;
            var model = okObjectResult?.Value as MarkDownModel;

            Assert.IsNotNull(okObjectResult);
            Assert.IsNotNull(model);
            Assert.That(model.Body, Is.EqualTo(document.MarkDown));
        }

        [Test]
        public async Task MarkDownController_Get_ServiceReturnsNull()
        {
            // Arrange
            DocumentServiceMoq.Setup(d => d.GetDocument()).ReturnsAsync((Document?)null);

            // Act
            var actionResult = await Controller.Get();

            // Assert
            var okObjectResult = actionResult as OkObjectResult;
            var model = okObjectResult?.Value as MarkDownModel;

            Assert.IsNotNull(okObjectResult);
            Assert.IsNotNull(model);
            Assert.That(model.Body, Is.EqualTo(string.Empty));
        }

        [Test]
        public async Task MarkDownController_Get_ServiceThrowsException()
        {
            // Arrange
            var exception = new Exception("exception raised by service");
            DocumentServiceMoq.Setup(d => d.GetDocument()).ThrowsAsync(exception);

            // Act
            var actionResult = await Controller.Get();

            // Assert
            var result = actionResult as ObjectResult;
            var model = result?.Value as Exception;

            Assert.IsNotNull(result);
            Assert.IsNotNull(model);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));
            Assert.That(model.Message, Is.EqualTo(exception.Message));
        }

        [Test]
        public async Task MarkDownController_Post_ModelNotValid()
        {
            // Arrange
            var model = new MarkDownModel
            {
                Body = string.Empty,
            };

            // Act
            var actionResult = await Controller.Post(model);

            // Assert
            var result = actionResult as BadRequestObjectResult;
            var modelResult = result?.Value as string;

            Assert.IsNotNull(result);
            Assert.IsNotNull(modelResult);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task MarkDownController_GetReadMe_ServiceReturnsDocument()
        {
            // Arrange
            var document = new Document
            {
                Id = 1,
                MarkDown = "# Hello World"
            };
            DocumentServiceMoq.Setup(d => d.GetReadMeDocument()).ReturnsAsync(document);

            // Act
            var actionResult = await Controller.GetReadMe();

            // Assert
            var okObjectResult = actionResult as OkObjectResult;
            var model = okObjectResult?.Value as MarkDownModel;

            Assert.IsNotNull(okObjectResult);
            Assert.IsNotNull(model);
            Assert.That(model.Body, Is.EqualTo(document.MarkDown));
        }

        [Test]
        public async Task MarkDownController_GetReadMe_ServiceReturnsNull()
        {
            // Arrange
            DocumentServiceMoq.Setup(d => d.GetReadMeDocument()).ReturnsAsync((Document?)null);

            // Act
            var actionResult = await Controller.GetReadMe();

            // Assert
            var okObjectResult = actionResult as OkObjectResult;
            var model = okObjectResult?.Value as MarkDownModel;

            Assert.IsNotNull(okObjectResult);
            Assert.IsNotNull(model);
            Assert.That(model.Body, Is.EqualTo(string.Empty));
        }

        [Test]
        public async Task MarkDownController_GetReadMe_ServiceThrowsException()
        {
            // Arrange
            var exception = new Exception("exception raised by service");
            DocumentServiceMoq.Setup(d => d.GetReadMeDocument()).ThrowsAsync(exception);

            // Act
            var actionResult = await Controller.GetReadMe();

            // Assert
            var result = actionResult as ObjectResult;
            var model = result?.Value as Exception;

            Assert.IsNotNull(result);
            Assert.IsNotNull(model);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));
            Assert.That(model.Message, Is.EqualTo(exception.Message));
        }

        [Test]
        public async Task MarkDownController_Post_ServiceReturnsNull()
        {
            // Arrange
            var model = new MarkDownModel
            {
                Body = "# Hello World",
            };
            DocumentServiceMoq.Setup(d => d.UpsertDocument(model.Body)).ReturnsAsync((Document?)null);

            // Act
            var actionResult = await Controller.Post(model);

            // Assert
            var result = actionResult as ObjectResult;
            var modelResult = result?.Value as Exception;

            Assert.IsNotNull(result);
            Assert.IsNotNull(modelResult);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));
        }

        [Test]
        public async Task MarkDownController_Post_ServiceReturnsDocumentWithEmptyMarkDown()
        {
            // Arrange
            var model = new MarkDownModel
            {
                Body = "# Hello World",
            };
            var document = new Document
            {
                Id = 1,
                MarkDown = null,
            };

            DocumentServiceMoq.Setup(d => d.UpsertDocument(model.Body)).ReturnsAsync(document);

            // Act
            var actionResult = await Controller.Post(model);

            // Assert
            var result = actionResult as ObjectResult;
            var modelResult = result?.Value as Exception;

            Assert.IsNotNull(result);
            Assert.IsNotNull(modelResult);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));
        }

        [Test]
        public async Task MarkDownController_Post_ServiceThrowsException()
        {
            // Arrange
            var model = new MarkDownModel
            {
                Body = "# Hello World",
            };
            var exception = new Exception("exception raised by service");

            DocumentServiceMoq.Setup(d => d.UpsertDocument(model.Body)).ThrowsAsync(exception);

            // Act
            var actionResult = await Controller.Post(model);

            // Assert
            var result = actionResult as ObjectResult;
            var modelResult = result?.Value as Exception;

            Assert.IsNotNull(result);
            Assert.IsNotNull(modelResult);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));
            Assert.That(modelResult, Is.EqualTo(exception));
        }

        [Test]
        public async Task MarkDownController_Post_ServiceReturnsDocument()
        {
            // Arrange
            var markDownBody = "# Hello World";
            var model = new MarkDownModel
            {
                Body = markDownBody,
            };
            var document = new Document
            {
                Id = 1,
                MarkDown = markDownBody,
            };

            DocumentServiceMoq.Setup(d => d.UpsertDocument(model.Body)).ReturnsAsync(document);

            // Act
            var actionResult = await Controller.Post(model);

            // Assert
            var result = actionResult as OkObjectResult;
            var modelResult = result?.Value as MarkDownModel;

            Assert.IsNotNull(result);
            Assert.IsNotNull(modelResult);
            Assert.That(model.Body, Is.EqualTo(modelResult.Body));
        }
    }
}
