using BlazorMarkDownAppJwt.Server.Entities;
using BlazorMarkDownAppJwt.Server.Services.Users;
using Microsoft.EntityFrameworkCore;

namespace BlazorMarkDownAppJwt.UnitTests.Server.Services
{
    public class UserServiceTest
    {
        private DataBaseContext Ctx { get; set; }

        private UserService Service { get; set; }

        private CancellationToken CancellationToken { get; set; }

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            Ctx = new DataBaseContext(options);
            Service = new UserService(this.Ctx);
            var source = new CancellationTokenSource();
            CancellationToken = source.Token;
        }

        [Test]
        public async Task UserService_AuthenticateUserAsync_ReturnsNull()
        {
            // Arrange
            var email = "email";
            var password = "password";

            // Act
            var result = await Service.AuthenticateUserAsync(email, password, CancellationToken);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task UserService_AuthenticateUserAsync_ReturnsUser()
        {
            // Arrange
            var email = "email";
            var password = "password";
            var existingUser = new User { Email = email, Password = password };
            this.Ctx.Users.Add(existingUser);
            this.Ctx.SaveChanges();

            // Act
            var result = await Service.AuthenticateUserAsync(email, password, CancellationToken);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result?.Email, Is.EqualTo(email));
                Assert.That(result?.Password, Is.EqualTo(password));
            });

            // Clean
            this.Ctx.Users.Remove(existingUser);
        }

        [Test]
        public async Task UserService_AddUserAsync_ReturnsUser()
        {
            // Arrange
            var email = "email";
            var password = "password";
            var firstName = "firstName";
            var lastName = "lastName";
            var newUser = new User
            { Email = email, Password = password, FirstName = firstName, LastName = lastName };

            // Act
            var result = await Service.AddUserAsync(newUser, CancellationToken);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.EqualTo(newUser));
            });


            // Clean
            this.Ctx.Users.Remove(newUser);
            this.Ctx.SaveChanges();
        }

        [Test]
        public async Task UserService_AddUserAsync_UserAlreadyExist_ReturnsNull()
        {
            // Arrange
            var email = "email";
            var password = "password";
            var firstName = "firstName";
            var lastName = "lastName";
            var newUser = new User
            { Id = 1, Email = email, Password = password, FirstName = firstName, LastName = lastName };
            this.Ctx.Users.Add(newUser);
            this.Ctx.SaveChanges();

            // Act
            var result = await Service.AddUserAsync(newUser, CancellationToken);

            // Assert
            Assert.That(result, Is.Null);

            // Clean
            Ctx.Users.Remove(newUser);
            Ctx.SaveChanges();
        }

        [Test]
        public async Task UserService_AddUserAsync_UserHaveEmptyInputs_ReturnsNull(
            [Values("email", "password", "firstName", "lastName")] string param1)
        {
            // Arrange
            var email = "email";
            var password = "password";
            var firstName = "firstName";
            var lastName = "lastName";

            switch (param1)
            {
                case "email":
                    email = string.Empty;
                    break;
                case "password":
                    password = string.Empty;
                    break;
                case "firstName":
                    firstName = string.Empty;
                    break;
                case "lastName":
                    lastName = string.Empty;
                    break;
                default:
                    break;
            }

            var newUser = new User
            { Email = email, Password = password, FirstName = firstName, LastName = lastName };

            // Act
            var result = await Service.AddUserAsync(newUser, CancellationToken);

            // Assert
            Assert.That(result, Is.Null);
        }
    }
}
