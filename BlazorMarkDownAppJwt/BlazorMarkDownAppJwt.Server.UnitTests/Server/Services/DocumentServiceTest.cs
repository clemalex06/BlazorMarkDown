using BlazorMarkDownAppJwt.Server.Entities;
using BlazorMarkDownAppJwt.Server.Services.MarkDowns;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorMarkDownAppJwt.UnitTests.Server.Services
{
    public class DocumentServiceTest
    {
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
            var result = await Service.GetDocument();

            // Assert
            Assert.IsNull(result);
        }
    }
}
