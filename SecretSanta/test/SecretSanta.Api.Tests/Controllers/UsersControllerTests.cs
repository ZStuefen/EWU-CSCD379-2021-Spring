using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using SecretSanta.Api.Controllers;
using System.Collections.Generic;
using SecretSanta.Business;
using SecretSanta.Data;
using Microsoft.AspNetCore.Mvc;
using System;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    public class UsersControllerTests
    {
        [TestMethod]
        //[ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_WithNullUserManager_ThrowsAppropriateException()
        {
            //Any of the approachs shown here are fine.
            ArgumentNullException ex = Assert.ThrowsException<ArgumentNullException>(
                () => new UsersController(null!));
            Assert.AreEqual("userManager", ex.ParamName);

            try
            {
                new UsersController(null!);
            }
            catch(ArgumentNullException e)
            {
                Assert.AreEqual("userManager", e.ParamName);
                return;
            }
            Assert.Fail("No exception thrown");
        }

        [TestMethod]
        public void Get_WithData_ReturnsUsers()
        {
            //Arrange
            UsersController controller = new(new UserManager());

            //Act
            IEnumerable<User> users = controller.Get();

            //Assert
            Assert.IsTrue(users.Any());
        }
    
        [TestMethod]
        [DataRow(42)]
        [DataRow(98)]
        public void Get_WithId_ReturnsUserManagerUser(int id)
        {
            //Arrange
            TestableUserManager manager = new();
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
            TestableUserManager manager = new();
            UsersController controller = new(manager);
            User expectedUser = new();
            manager.GetItemUser = expectedUser;

            //Act
            ActionResult<User?> result = controller.Get(-1);

            //Assert
            Assert.IsTrue(result.Result is NotFoundResult);
        }

        private class TestableUserManager : IUserRepository
        {
            public User Create(User item)
            {
                throw new System.NotImplementedException();
            }

            public User? GetItemUser { get; set; }
            public int GetItemId { get; set; }
            public User? GetItem(int id)
            {
                GetItemId = id;
                return GetItemUser;
            }

            public ICollection<User> List()
            {
                throw new System.NotImplementedException();
            }

            public bool Remove(int id)
            {
                throw new System.NotImplementedException();
            }

            public void Save(User item)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}