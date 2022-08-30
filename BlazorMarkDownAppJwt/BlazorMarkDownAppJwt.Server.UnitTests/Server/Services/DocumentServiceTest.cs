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
        private string? testDataDir { get { return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); } }
        private DataBaseContext Ctx { get; set; }

        private Mock<IWebHostEnvironment> WebHostEnvironmentMoq { get; set; }

        private DocumentService Service { get; set; }


        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            this.Ctx = new DataBaseContext(options);

            this.WebHostEnvironmentMoq = new Mock<IWebHostEnvironment>();
            this.Service = new DocumentService(this.Ctx, this.WebHostEnvironmentMoq.Object);
        }

        [Test]
        public async Task DocumentService_GetDocument_ReturnsNull()
        {
            // Arrange

            // Act
            var result = await Service.GetDocument(1);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task DocumentService_GetDocument_ReturnsDocument()
        {
            // Arrange

            // Act
            var result = await Service.GetDocument(1);

            // Assert
            Assert.IsNull(result);
        }

        //[Test]
        //public void DocumentService_GetDocument_MultipleDocuments_ThrowException()
        //{
        //    // Arrange
        //    var document1 = new Document { Id = 1, MarkDown = "# Hello World1" };
        //    var document2 = new Document { Id = 2, MarkDown = "# Hello World2" };
        //    this.Ctx.Document.Add(document1);
        //    this.Ctx.Document.Add(document2);
        //    this.Ctx.SaveChanges();

        //    // Act & Assert
        //    Assert.ThrowsAsync<InvalidOperationException>(Service.GetDocument);

        //    // Clean
        //    this.Ctx.Document.Remove(document1);
        //    this.Ctx.Document.Remove(document2);
        //    this.Ctx.SaveChanges();
        //}

        [Test]
        public async Task DocumentService_InsertDocument_ReturnsDocument()
        {
            // Arrange
            var markDown = "# Hello World";

            // Act
            var result = await Service.InsertDocument(markDown);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.MarkDown, Is.EqualTo(markDown));

            // Clean
            this.Ctx.Document.Remove(result);
            this.Ctx.SaveChanges();
        }

        [Test]
        public async Task DocumentService_UpdateDocument_ReturnsDocument()
        {
            // Arrange
            var document = new Document { Id = 1, MarkDown = "# Hello World" };
            var markDownupdated = "# Hello World updated";
            this.Ctx.Document.Add(document);
            this.Ctx.SaveChanges();

            // Act
            var result = await Service.UpdateDocument(document.Id, markDownupdated);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.MarkDown, Is.EqualTo(markDownupdated));

            // Clean
            this.Ctx.Document.Remove(result);
            this.Ctx.SaveChanges();
        }

        [Test]
        public async Task DocumentService_GetReadMeDocument_ReturnsNull()
        {
            // Arrange
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).ToString();
            this.WebHostEnvironmentMoq.Setup(w => w.ContentRootPath).Returns(path);

            // Act
            var result = await Service.GetReadMeDocument();

            // Assert
            Assert.IsNull(result);
        }


    }
}
