using BlazorMarkDownAppJwt.Server.Controllers;
using BlazorMarkDownAppJwt.Server.Services.MarkDowns;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public void Test1()
        {
            // Arrange
            // Act
            // Assert
            Assert.Pass();
        }
    }
}
