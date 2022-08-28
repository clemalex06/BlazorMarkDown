using BlazorMarkDownAppJwt.Server.Entities;
using BlazorMarkDownAppJwt.Server.Services.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorMarkDownAppJwt.UnitTests.Server.Services
{
    public class UserServiceTest
    {
        private DataBaseContext Ctx { get; set; }

        private UserService Service { get; set; }

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            this.Ctx = new DataBaseContext(options);
            this.Service = new UserService(this.Ctx);
        }

        [Test]
        public async Task UserService_AuthenticateUser_ReturnsNull()
        {
            // Arrange
            var email = "email";
            var password = "password";

            // Act
            var result = await Service.AuthenticateUser(email, password);

            // Assert
            Assert.IsNull(result);
        }
    }
}
