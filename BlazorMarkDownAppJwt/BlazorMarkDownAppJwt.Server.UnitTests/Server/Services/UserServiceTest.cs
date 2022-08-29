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

        [Test]
        public async Task UserService_AuthenticateUser_ReturnsUser()
        {
            // Arrange
            var email = "email";
            var password = "password";
            var existingUser = new User { Email = email, Password = password };
            this.Ctx.Users.Add(existingUser);
            this.Ctx.SaveChanges();

            // Act
            var result = await Service.AuthenticateUser(email, password);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Email, Is.EqualTo(email));
            Assert.That(result.Password, Is.EqualTo(password));

            // Clean
            this.Ctx.Users.Remove(existingUser);
        }

        [Test]
        public async Task UserService_AddUser_ReturnsUser()
        {
            // Arrange
            var email = "email";
            var password = "password";
            var firstName = "firstName";
            var lastName = "lastName";
            var newUser = new User
            { Email = email, Password = password, FirstName = firstName, LastName = lastName };

            // Act
            var result = await Service.AddUser(newUser);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result, Is.EqualTo(newUser));

            // Clean
            this.Ctx.Users.Remove(newUser);
            this.Ctx.SaveChanges();
        }

        [Test]
        public async Task UserService_AddUser_UserAlreadyExist_ReturnsNull()
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
            var result = await Service.AddUser(newUser);

            // Assert
            Assert.IsNull(result);

            // Clean
            this.Ctx.Users.Remove(newUser);
            this.Ctx.SaveChanges();
        }

        [Test]
        public async Task UserService_AddUser_UserHaveEmptyInputs_ReturnsNull(
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
            var result = await Service.AddUser(newUser);

            // Assert
            Assert.IsNull(result);
        }
    }
}
