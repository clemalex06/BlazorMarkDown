using BlazorMarkDownAppJwt.Server.Controllers;
using BlazorMarkDownAppJwt.Server.Entities;
using BlazorMarkDownAppJwt.Server.Services.Users;
using BlazorMarkDownAppJwt.Shared;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BlazorMarkDownAppJwt.UnitTests.Server.Controllers
{
    public class AuthControllerTest
    {
        private Mock<IUserService> UserServiceMoq { get; set; }

        private AuthController Controller { get; set; }

        private CancellationToken CancellationToken { get; set; }


        [SetUp]
        public void Setup()
        {
            UserServiceMoq = new Mock<IUserService>();
            Controller = new AuthController(UserServiceMoq.Object);
            var source = new CancellationTokenSource();
            CancellationToken = source.Token;
        }

        [Test]
        public async Task AuthController_Register_PassWords_NoMatch()
        {
            // Arrange
            var model = new RegModel
            {
                Email = "a@a.fr",
                FirstName = "firstName",
                LastName = "lastName",
                Password = "password",
                Confirmpwd = "confirmpwd",
            };

            // Act
            var actionResult = await Controller.PostAsync(model, CancellationToken);

            // Assert
            var okObjectResult = actionResult.Result as OkObjectResult;
            var modelResult = okObjectResult?.Value as LoginResult;
            Assert.That(modelResult, Is.Not.Null);
            Assert.That(modelResult.Success, Is.False);
        }

        [Test]
        public async Task AuthController_Register_ServiceReturnsNull()
        {
            // Arrange
            var model = new RegModel
            {
                Email = "a@a.fr",
                FirstName = "firstName",
                LastName = "lastName",
                Password = "password",
                Confirmpwd = "password",
            };
            UserServiceMoq.Setup(p => p.AddUserAsync(It.IsAny<User>(), CancellationToken)).ReturnsAsync((User?)null);

            // Act
            var actionResult = await Controller.PostAsync(model, CancellationToken);

            // Assert
            var okObjectResult = actionResult.Result as OkObjectResult;
            var modelResult = okObjectResult?.Value as LoginResult;
            Assert.That(modelResult, Is.Not.Null);
            Assert.That(modelResult.Success, Is.False);
        }

        [Test]
        public async Task AuthController_Register_ServiceReturnsUser()
        {
            // Arrange
            var model = new RegModel
            {
                Email = "a@a.fr",
                FirstName = "firstName",
                LastName = "lastName",
                Password = "password",
                Confirmpwd = "password",
            };
            UserServiceMoq.Setup(p => p.AddUserAsync(It.IsAny<User>(), CancellationToken)).ReturnsAsync(new User
            {
                Email = model.Email,
                FirstName = model.FirstName,
                Id = 1,
                LastName = model.LastName,
                Password = model.Password,
            });

            // Act
            var actionResult = await Controller.PostAsync(model, CancellationToken);

            // Assert
            var okObjectResult = actionResult.Result as OkObjectResult;
            var modelResult = okObjectResult?.Value as LoginResult;
            Assert.That(modelResult, Is.Not.Null);
            Assert.That(modelResult.Success, Is.True);
        }

        [Test]
        public async Task AuthController_Register_ServiceThrowsException()
        {
            // Arrange
            var model = new RegModel
            {
                Email = "a@a.fr",
                FirstName = "firstName",
                LastName = "lastName",
                Password = "password",
                Confirmpwd = "password",
            };
            var exceptionMessage = "exception raised by service";

            UserServiceMoq.Setup(p => p.AddUserAsync(It.IsAny<User>(), CancellationToken)).ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var actionResult = await Controller.PostAsync(model, CancellationToken);

            // Assert
            var okObjectResult = actionResult.Result as OkObjectResult;
            var modelResult = okObjectResult?.Value as LoginResult;
            Assert.Multiple(() =>
            {
                Assert.That(modelResult, Is.Not.Null);
                Assert.That(modelResult?.Success, Is.False);
                Assert.That(modelResult?.Message, Is.EqualTo(exceptionMessage));
            });
        }

        [Test]
        public async Task AuthController_Login_ServiceReturnsUser()
        {
            // Arrange
            var model = new LoginModel
            {
                Email = "a@a.fr",
                Password = "password",
            };
            var firstName = "firstName";
            var lastName = "lastName";

            UserServiceMoq.Setup(p => p.AuthenticateUserAsync(model.Email, model.Password, CancellationToken)).ReturnsAsync(new User
            {
                Email = model.Email,
                FirstName = firstName,
                Id = 1,
                LastName = lastName,
                Password = model.Password,
            });

            // Act
            var actionResult = await Controller.PostAsync(model, CancellationToken);

            // Assert
            var okObjectResult = actionResult.Result as OkObjectResult;
            var modelResult = okObjectResult?.Value as LoginResult;
            Assert.That(modelResult, Is.Not.Null);
            Assert.That(modelResult?.Success, Is.True);
        }

        [Test]
        public async Task AuthController_Login_ServiceReturnsNull()
        {
            // Arrange
            var model = new LoginModel
            {
                Email = "a@a.fr",
                Password = "password",
            };

            UserServiceMoq.Setup(p => p.AuthenticateUserAsync(model.Email, model.Password, CancellationToken)).ReturnsAsync((User?)null);

            // Act
            var actionResult = await Controller.PostAsync(model, CancellationToken);

            // Assert
            var okObjectResult = actionResult.Result as OkObjectResult;
            var modelResult = okObjectResult?.Value as LoginResult;
            Assert.That(modelResult, Is.Not.Null);
            Assert.That(modelResult.Success, Is.False);
        }

        [Test]
        public async Task AuthController_Login_ServiceThrowsException()
        {
            // Arrange
            var model = new LoginModel
            {
                Email = "a@a.fr",
                Password = "password",
            };
            var exceptionMessage = "exception raised by service";

            UserServiceMoq.Setup(p => p.AuthenticateUserAsync(model.Email, model.Password, CancellationToken)).ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var actionResult = await Controller.PostAsync(model, CancellationToken);

            // Assert
            var okObjectResult = actionResult.Result as OkObjectResult;
            var modelResult = okObjectResult?.Value as LoginResult;
            Assert.That(modelResult, Is.Not.Null);
            Assert.That(modelResult.Success, Is.False);
        }
    }
}
