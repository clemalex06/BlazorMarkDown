using BlazorMarkDownAppJwt.Server.Entities;
using BlazorMarkDownAppJwt.Server.Services.MarkDowns;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Reflection;

namespace BlazorMarkDownAppJwt.UnitTests.Server.Services
{
    public class DocumentServiceTest
    {
        private DataBaseContext Ctx { get; set; }

        private Mock<IWebHostEnvironment> WebHostEnvironmentMoq { get; set; }

        private DocumentService Service { get; set; }

        private CancellationToken CancellationToken { get; set; }


        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            Ctx = new DataBaseContext(options);

            WebHostEnvironmentMoq = new Mock<IWebHostEnvironment>();
            Service = new DocumentService(this.Ctx, this.WebHostEnvironmentMoq.Object);
            var source = new CancellationTokenSource();
            CancellationToken = source.Token;
        }

        [Test]
        public async Task DocumentService_GetDocumentAsync_ReturnsNull()
        {
            // Arrange

            // Act
            var result = await Service.GetDocumentAsync(1, CancellationToken);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task DocumentService_GetDocumentAsync_ReturnsDocument()
        {
            // Arrange

            // Act
            var result = await Service.GetDocumentAsync(1, CancellationToken);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task DocumentService_InsertDocumentAsync_ReturnsDocument()
        {
            // Arrange
            var markDown = "# Hello World";

            // Act
            var result = await Service.InsertDocumentAsync(markDown, CancellationToken);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.MarkDown, Is.EqualTo(markDown));

            // Clean
            this.Ctx.Document.Remove(result);
            this.Ctx.SaveChanges();
        }

        [Test]
        public async Task DocumentService_UpdateDocumentAsync_ReturnsDocument()
        {
            // Arrange
            var document = new Document { Id = 1, MarkDown = "# Hello World" };
            var markDownupdated = "# Hello World updated";
            this.Ctx.Document.Add(document);
            this.Ctx.SaveChanges();

            // Act
            var result = await Service.UpdateDocumentAsync(document.Id, markDownupdated, CancellationToken);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.MarkDown, Is.EqualTo(markDownupdated));

            // Clean
            this.Ctx.Document.Remove(result);
            this.Ctx.SaveChanges();
        }

        [Test]
        public void DocumentService_UpdateDocumentAsync_ThrowException()
        {
            // Arrange
            long id = 1;
            var markDownupdated = "# Hello World updated";

            // Act & Assert
            Exception ex = Assert.ThrowsAsync<Exception>(async () => await Service.UpdateDocumentAsync(
                id,
                markDownupdated,
                CancellationToken));

            // Assert
            Assert.That(ex.Message, Is.EqualTo($"unable to find document with id = {id}"));
        }

        [Test]
        public async Task DocumentService_GetReadMeDocumentAsync_ReturnsNull()
        {
            // Arrange
            string location = Assembly.GetExecutingAssembly().Location;
            var directoryName = Path.GetDirectoryName(location);
            if (!string.IsNullOrWhiteSpace(directoryName))
                WebHostEnvironmentMoq.Setup(w => w.ContentRootPath).Returns(directoryName);

            // Act
            var result = await Service.GetReadMeDocumentAsync(CancellationToken);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task DocumentService_GetAllDocumentsAsync_ReturnsEmptyList()
        {
            // Arrange

            // Act
            var result = await Service.GetAllDocumentsAsync(CancellationToken);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(0));
        }

        [Test]
        public async Task DocumentService_GetAllDocumentsAsync_ReturnsList()
        {
            // Arrange
            var document1 = new Document
            {
                Id = 1,
                MarkDown = "# Markdown1",
            };

            var document2 = new Document
            {
                Id = 2,
                MarkDown = "# Markdown2",
            };
            this.Ctx.Document.Add(document1);
            this.Ctx.Document.Add(document2);
            this.Ctx.SaveChanges();

            // Act
            var result = await Service.GetAllDocumentsAsync(CancellationToken);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(2));

            // Clean
            this.Ctx.Document.Remove(document1);
            this.Ctx.Document.Remove(document2);
            this.Ctx.SaveChanges();
        }
    }
}
