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

        private CancellationToken CancellationToken { get; set; }

        [SetUp]
        public void Setup()
        {
            DocumentServiceMoq = new Mock<IDocumentService>();
            Controller = new MarkDownController(DocumentServiceMoq.Object);
            CancellationToken = new CancellationToken();
        }

        [Test]
        public async Task MarkDownController_GetAsync_ServiceReturnsDocument()
        {
            // Arrange
            var document = new Document
            {
                Id = 1,
                MarkDown = "# Hello World"
            };
            DocumentServiceMoq.Setup(d => d.GetDocumentAsync(document.Id, CancellationToken)).ReturnsAsync(document);

            // Act
            var actionResult = await Controller.GetAsync(document.Id, CancellationToken);

            // Assert
            var okObjectResult = actionResult.Result as OkObjectResult;
            var model = okObjectResult?.Value as MarkDownModel;

            Assert.IsNotNull(okObjectResult);
            Assert.IsNotNull(model);
            Assert.That(model.Body, Is.EqualTo(document.MarkDown));
        }

        [Test]
        public async Task MarkDownController_GetAsync_ServiceReturnsNull()
        {
            // Arrange
            DocumentServiceMoq.Setup(d => d.GetDocumentAsync(1, CancellationToken)).ReturnsAsync((Document?)null);

            // Act
            var actionResult = await Controller.GetAsync(1, CancellationToken);

            // Assert
            var notFoundResult = actionResult.Result as NotFoundResult;
            Assert.IsNotNull(notFoundResult);
        }

        [Test]
        public async Task MarkDownController_GetAsync_ServiceThrowsException()
        {
            // Arrange
            var exception = new Exception("exception raised by service");
            DocumentServiceMoq.Setup(d => d.GetDocumentAsync(1, CancellationToken)).ThrowsAsync(exception);

            // Act
            var actionResult = await Controller.GetAsync(1, CancellationToken);

            // Assert
            var result = actionResult.Result as ObjectResult;
            var model = result?.Value as Exception;

            Assert.IsNotNull(result);
            Assert.IsNotNull(model);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));
            Assert.That(model.Message, Is.EqualTo(exception.Message));
        }

        [Test]
        public async Task MarkDownController_GetReadMeAsync_ServiceReturnsDocument()
        {
            // Arrange
            var document = new Document
            {
                Id = 1,
                MarkDown = "# Hello World"
            };
            DocumentServiceMoq.Setup(d => d.GetReadMeDocumentAsync(CancellationToken)).ReturnsAsync(document);

            // Act
            var actionResult = await Controller.GetReadMeAsync(CancellationToken);

            // Assert
            var okObjectResult = actionResult.Result as OkObjectResult;
            var model = okObjectResult?.Value as MarkDownModel;

            Assert.IsNotNull(okObjectResult);
            Assert.IsNotNull(model);
            Assert.That(model.Body, Is.EqualTo(document.MarkDown));
        }

        [Test]
        public async Task MarkDownController_GetReadMeAsync_ServiceReturnsNull()
        {
            // Arrange
            DocumentServiceMoq.Setup(d => d.GetReadMeDocumentAsync(CancellationToken)).ReturnsAsync((Document?)null);

            // Act
            var actionResult = await Controller.GetReadMeAsync(CancellationToken);

            // Assert
            var notFoundResult = actionResult.Result as NotFoundResult;
            Assert.IsNotNull(notFoundResult);
        }

        [Test]
        public async Task MarkDownController_GetReadMeAsync_ServiceThrowsException()
        {
            // Arrange
            var exception = new Exception("exception raised by service");
            DocumentServiceMoq.Setup(d => d.GetReadMeDocumentAsync(CancellationToken)).ThrowsAsync(exception);

            // Act
            var actionResult = await Controller.GetReadMeAsync(CancellationToken);

            // Assert
            var result = actionResult.Result as ObjectResult;
            var model = result?.Value as Exception;

            Assert.IsNotNull(result);
            Assert.IsNotNull(model);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));
            Assert.That(model.Message, Is.EqualTo(exception.Message));
        }

        [Test]
        public async Task MarkDownController_PostAsync_ModelNotValid()
        {
            // Arrange
            var model = new MarkDownModel
            {
                Body = string.Empty,
            };

            // Act
            var actionResult = await Controller.PostAsync(model, CancellationToken);

            // Assert
            var result = actionResult.Result as BadRequestObjectResult;
            var modelResult = result?.Value as string;

            Assert.IsNotNull(result);
            Assert.IsNotNull(modelResult);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task MarkDownController_PostAsync_ServiceThrowsException()
        {
            // Arrange
            var model = new MarkDownModel
            {
                Id = 1,
                Body = "# Hello World",
            };
            var exception = new Exception("exception raised by service");

            DocumentServiceMoq.Setup(d => d.UpdateDocumentAsync(model.Id, model.Body, CancellationToken)).ThrowsAsync(exception);

            // Act
            var actionResult = await Controller.PostAsync(model, CancellationToken);

            // Assert
            var result = actionResult.Result as ObjectResult;
            var modelResult = result?.Value as Exception;

            Assert.IsNotNull(result);
            Assert.IsNotNull(modelResult);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));
            Assert.That(modelResult, Is.EqualTo(exception));
        }

        [Test]
        public async Task MarkDownController_PostAsync_ServiceReturnsDocument()
        {
            // Arrange
            var markDownBody = "# Hello World";
            var model = new MarkDownModel
            {
                Id = 1,
                Body = markDownBody,
            };
            var document = new Document
            {
                Id = 1,
                MarkDown = markDownBody,
            };

            DocumentServiceMoq.Setup(d => d.UpdateDocumentAsync(model.Id, model.Body, CancellationToken)).ReturnsAsync(document);

            // Act
            var actionResult = await Controller.PostAsync(model, CancellationToken);

            // Assert
            var result = actionResult.Result as OkObjectResult;
            var modelResult = result?.Value as MarkDownModel;

            Assert.IsNotNull(result);
            Assert.IsNotNull(modelResult);
            Assert.That(model.Body, Is.EqualTo(modelResult.Body));
        }

        [Test]
        public async Task MarkDownController_PutAsync_ModelNotValid()
        {
            // Arrange
            var model = new MarkDownModel
            {
                Body = string.Empty,
            };

            // Act
            var actionResult = await Controller.PutAsync(model, CancellationToken);

            // Assert
            var result = actionResult.Result as BadRequestObjectResult;
            var modelResult = result?.Value as string;

            Assert.IsNotNull(result);
            Assert.IsNotNull(modelResult);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task MarkDownController_PutAsync_ServiceThrowsException()
        {
            // Arrange
            var model = new MarkDownModel
            {
                Id = 1,
                Body = "# Hello World",
            };
            var exception = new Exception("exception raised by service");

            DocumentServiceMoq.Setup(d => d.InsertDocumentAsync(model.Body, CancellationToken)).ThrowsAsync(exception);

            // Act
            var actionResult = await Controller.PutAsync(model, CancellationToken);

            // Assert
            var result = actionResult.Result as ObjectResult;
            var modelResult = result?.Value as Exception;

            Assert.IsNotNull(result);
            Assert.IsNotNull(modelResult);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));
            Assert.That(modelResult, Is.EqualTo(exception));
        }

        [Test]
        public async Task MarkDownController_PutAsync_ServiceReturnsDocument()
        {
            // Arrange
            var markDownBody = "# Hello World";
            var model = new MarkDownModel
            {
                Id = 1,
                Body = markDownBody,
            };
            var document = new Document
            {
                Id = 1,
                MarkDown = markDownBody,
            };

            DocumentServiceMoq.Setup(d => d.InsertDocumentAsync(model.Body, CancellationToken)).ReturnsAsync(document);

            // Act
            var actionResult = await Controller.PutAsync(model, CancellationToken);

            // Assert
            var result = actionResult.Result as OkObjectResult;
            var modelResult = result?.Value as MarkDownModel;

            Assert.IsNotNull(result);
            Assert.IsNotNull(modelResult);
            Assert.That(model.Body, Is.EqualTo(modelResult.Body));
        }

        [Test]
        public async Task MarkDownController_GetAllAsync_ServiceReturnsEmtpyList()
        {
            // Arrange
            var listDocument = new List<Document>();

            DocumentServiceMoq.Setup(d => d.GetAllDocumentsAsync(CancellationToken)).ReturnsAsync(listDocument);

            // Act
            var actionResult = await Controller.GetAllAsync(CancellationToken);

            // Assert
            var result = actionResult.Result as OkObjectResult;
            var modelResult = result?.Value as List<MarkDownModel>;

            Assert.IsNotNull(result);
            Assert.IsNotNull(modelResult);
            Assert.That(listDocument.Count, Is.EqualTo(modelResult.Count));

        }

        [Test]
        public async Task MarkDownController_GetAllAsync_ServiceReturnsList()
        {
            // Arrange
            var listDocument = new List<Document>();
            var document1 = new Document
            {
                Id = 1,
                MarkDown = "Hello",
            };

            var document2 = new Document
            {
                Id = 2,
                MarkDown = "Hello",
            };

            listDocument.Add(document1);
            listDocument.Add(document2);

            DocumentServiceMoq.Setup(d => d.GetAllDocumentsAsync(CancellationToken)).ReturnsAsync(listDocument);

            // Act
            var actionResult = await Controller.GetAllAsync(CancellationToken);

            // Assert
            var result = actionResult.Result as OkObjectResult;
            var modelResult = result?.Value as List<MarkDownModel>;

            Assert.IsNotNull(result);
            Assert.IsNotNull(modelResult);
            Assert.That(listDocument.Count, Is.EqualTo(modelResult.Count));
        }

        [Test]
        public async Task MarkDownController_GetAllAsync_ServiceServiceThrowsException()
        {
            // Arrange
            var exception = new Exception("exception raised by service");

            DocumentServiceMoq.Setup(d => d.GetAllDocumentsAsync(CancellationToken)).ThrowsAsync(exception);

            // Act
            var actionResult = await Controller.GetAllAsync(CancellationToken);

            // Assert
            var result = actionResult.Result as ObjectResult;
            var model = result?.Value as Exception;

            Assert.IsNotNull(result);
            Assert.IsNotNull(model);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.InternalServerError));
            Assert.That(model.Message, Is.EqualTo(exception.Message));
        }
    }
}
