using BlazorMarkDownAppJwt.Server.Controllers;
using BlazorMarkDownAppJwt.Server.Entities;
using BlazorMarkDownAppJwt.Server.Services.Users;
using BlazorMarkDownAppJwt.Shared;
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
        public async Task AuthController_Register_PassWords_NoMatch()
        {
            // Arrange
            var model = new RegModel
            {
                email = "a@a.fr",
                firstName = "firstName",
                lastName = "lastName",
                password = "password",
                confirmpwd = "confirmpwd",
            };

            // Act
            var result = await Controller.Post(model);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.success);
        }

        [Test]
        public async Task AuthController_Register_ServiceReturnsNull()
        {
            // Arrange
            var model = new RegModel
            {
                email = "a@a.fr",
                firstName = "firstName",
                lastName = "lastName",
                password = "password",
                confirmpwd = "password",
            };
            UserServiceMoq.Setup(p => p.AddUser(It.IsAny<User>())).ReturnsAsync((User?)null);

            // Act
            var result = await Controller.Post(model);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.success);
        }

        [Test]
        public async Task AuthController_Register_ServiceReturnsUser()
        {
            // Arrange
            var model = new RegModel
            {
                email = "a@a.fr",
                firstName = "firstName",
                lastName = "lastName",
                password = "password",
                confirmpwd = "password",
            };
            UserServiceMoq.Setup(p => p.AddUser(It.IsAny<User>())).ReturnsAsync(new User 
            {
                Email = model.email,
                FirstName = model.firstName,
                Id = 1,
                LastName = model.lastName,
                Password = model.password,
            });

            // Act
            var result = await Controller.Post(model);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.success);
        }

        [Test]
        public async Task AuthController_Register_ServiceThrowsException()
        {
            // Arrange
            var model = new RegModel
            {
                email = "a@a.fr",
                firstName = "firstName",
                lastName = "lastName",
                password = "password",
                confirmpwd = "password",
            };
            var exceptionMessage = "exception raised by service";

            UserServiceMoq.Setup(p => p.AddUser(It.IsAny<User>())).ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await Controller.Post(model);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.success);
            Assert.That(result.message, Is.EqualTo(exceptionMessage));
        }

        [Test]
        public async Task AuthController_Login_ServiceReturnsUser()
        {
            // Arrange
            var model = new LoginModel
            {
                email = "a@a.fr",
                password = "password",
            };
            var firstName = "firstName";
            var lastName = "lastName";

            UserServiceMoq.Setup(p => p.AuthenticateUser(model.email, model.password)).ReturnsAsync(new User
            {
                Email = model.email,
                FirstName = firstName,
                Id = 1,
                LastName = lastName,
                Password = model.password,
            });

            // Act
            var result = await Controller.Post(model);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.success);
        }

        [Test]
        public async Task AuthController_Login_ServiceReturnsNull()
        {
            // Arrange
            var model = new LoginModel
            {
                email = "a@a.fr",
                password = "password",
            };

            UserServiceMoq.Setup(p => p.AuthenticateUser(model.email, model.password)).ReturnsAsync((User?) null);

            // Act
            var result = await Controller.Post(model);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.success);
        }

        [Test]
        public async Task AuthController_Login_ServiceThrowsException()
        {
            // Arrange
            var model = new LoginModel
            {
                email = "a@a.fr",
                password = "password",
            };
            var exceptionMessage = "exception raised by service";

            UserServiceMoq.Setup(p => p.AuthenticateUser(model.email, model.password)).ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await Controller.Post(model);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.success);
        }
    }
}
