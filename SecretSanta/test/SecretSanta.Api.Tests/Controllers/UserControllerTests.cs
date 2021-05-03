using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using SecretSanta.Api.Controllers;
using System.Collections.Generic;
using SecretSanta.Business;
using SecretSanta.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using SecretSanta.Api.Dto;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SecretSanta.Api.Tests.Business;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    public class UsersControllerTests
    {
        [TestMethod]
        //[ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_WithNullUserRepository_ThrowsAppropriateException()
        {
            //Any of the approachs shown here are fine.
            ArgumentNullException ex = Assert.ThrowsException<ArgumentNullException>(
                () => new UsersController(null!));
            Assert.AreEqual("userRepository", ex.ParamName);

            try
            {
                new UsersController(null!);
            }
            catch(ArgumentNullException e)
            {
                Assert.AreEqual("userRepository", e.ParamName);
                return;
            }
            Assert.Fail("No exception thrown");
        }

        [TestMethod]
        public void Get_WithData_ReturnsUsers()
        {
            //Arrange
            UsersController controller = new(new UserRepository());

            //Act
            IEnumerable<User> users = controller.Get();

            //Assert
            Assert.IsTrue(users.Any());
        }
    
        [TestMethod]
        [DataRow(42)]
        [DataRow(98)]
        public void Get_WithId_ReturnsUserRepositoryUser(int id)
        {
            //Arrange
            TestableUserRepository manager = new();
            UsersController controller = new(manager);
            User expectedUser = new();
            manager.GetItemUser = expectedUser;

            //Act
            ActionResult<User?> result = controller.Get(id);

            //Assert
            Assert.AreEqual(id, manager.GetItemId);
            Assert.AreEqual(expectedUser, result.Value);
        }

        [TestMethod]
        public void Get_WithNegativeId_ReturnsNotFound()
        {
            //Arrange
            TestableUserRepository manager = new();
            UsersController controller = new(manager);
            User expectedUser = new();
            manager.GetItemUser = expectedUser;

            //Act
            ActionResult<User?> result = controller.Get(-1);

            //Assert
            Assert.IsTrue(result.Result is NotFoundResult);
        }

        [TestMethod]
        public async Task Put_WithValidData_UpdatesUser()
        {
            //Arrange
            WebApplicationFactory factory = new();
            TestableUserRepository manager = factory.Manager;
            User foundUser = new User
            {
                Id = 42
            };
            manager.GetItemUser = foundUser;

            HttpClient client = factory.CreateClient();
            UpdateUser updateUser = new()
            {
                FirstName = "Casey's",
                LastName = "Birthday"
            };

            //Act
            HttpResponseMessage response = await client.PutAsJsonAsync("/api/users/42", updateUser);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.AreEqual("Casey's", manager.SavedUser?.FirstName);
            Assert.AreEqual("Birthday", manager.SavedUser?.LastName);
        }
    }
}