using BlazorMarkDownAppJwt.Server.Controllers;
using BlazorMarkDownAppJwt.Server.Services.Users;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorMarkDownAppJwt.UnitTests.Server.Controllers
{
    public class AuthControllerTest
    {
        private Mock<IUserService> UserServiceMoq { get; set; }

        private AuthController Controller { get; set; }


        [SetUp]
        public void Setup()
        {
            UserServiceMoq = new Mock<IUserService>();
            Controller = new AuthController(UserServiceMoq.Object);
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
