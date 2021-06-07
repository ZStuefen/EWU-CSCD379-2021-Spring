using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using SecretSanta.Data;

namespace SecretSanta.Business.Tests
{
    [TestClass]
    public class GroupRepositoryTests
    {
        [TestCleanup]
        async public Task Clear_Database()
        {
            using DbContext dbContext = new DbContext();
            IQueryable<Group>? groups = dbContext.Groups.Where(
                item => item.Name.StartsWith(""));
            dbContext.Groups.RemoveRange(groups);
            await dbContext.SaveChangesAsync();
            
            IQueryable<User>? users = dbContext.Users.Where(
                item => item.FirstName.StartsWith(""));
            dbContext.Users.RemoveRange(users);
            await dbContext.SaveChangesAsync();
            
            IQueryable<Assignment>? a = dbContext.Assignments.Where(
                item => item.ToString()!.StartsWith(""));
            dbContext.Assignments.RemoveRange(a);
            await dbContext.SaveChangesAsync();
            
            IQueryable<GroupAssignment>? g = dbContext.GroupAssignments.Where(
                item => item.Group.Name.StartsWith(""));
            dbContext.GroupAssignments.RemoveRange(g);
            await dbContext.SaveChangesAsync();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_NullItem_ThrowsArgumentException()
        {
            GroupRepository sut = new();

            sut.Create(null!);
        }

        [TestMethod]
        public void Create_WithItem_CanGetItem()
        {
            GroupRepository sut = new();
            Group user = new()
            {
                Id = 42,
                Name = "Group",
            };

            Group createdGroup = sut.Create(user);

            Group? retrievedGroup = sut.GetItem(createdGroup.Id);
            Assert.AreEqual(user.Id, retrievedGroup!.Id);
            Assert.AreEqual(user.Name, retrievedGroup.Name);
        }

        [TestMethod]
        public void GetItem_WithBadId_ReturnsNull()
        {
            GroupRepository sut = new();

            Group? user = sut.GetItem(-1);

            Assert.IsNull(user);
        }

        [TestMethod]
        public void GetItem_WithValidId_ReturnsGroup()
        {
            GroupRepository sut = new();
            sut.Create(new() 
            { 
                Id = 42,
                Name = "Group",
            });

            Group? user = sut.GetItem(42);

            Assert.AreEqual(42, user?.Id);
            Assert.AreEqual("Group", user!.Name);
        }

        [TestMethod]
        public void List_WithGroups_ReturnsAllGroup()
        {
            using DbContext dbContext = new DbContext();
            GroupRepository sut = new();
            sut.Create(new()
            {
                Id = 42,
                Name = "Group",
            });

            ICollection<Group> users = sut.List();

            Assert.AreEqual(dbContext.Groups.Count(), users.Count);
            foreach(var mockGroup in dbContext.Groups)
            {
                Assert.IsNotNull(users.SingleOrDefault(x => x.Name == mockGroup.Name));
            }
        }

        [TestMethod]
        [DataRow(-1, false)]
        [DataRow(42, true)]
        public void Remove_WithInvalidId_ReturnsTrue(int id, bool expected)
        {
            GroupRepository sut = new();
            sut.Create(new()
            {
                Id = 42,
                Name = "Group"
            });

            Assert.AreEqual(expected, sut.Remove(id));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Save_NullItem_ThrowsArgumentException()
        {
            GroupRepository sut = new();

            sut.Save(new Group()!);
        }

        [TestMethod]
        public void Save_WithValidItem_SavesItem()
        {
            GroupRepository sut = new();

            sut.Save(new Group() { Name = "Group", Id = 42 });

            Assert.AreEqual(42, sut.GetItem(42)?.Id);
        }

        [TestMethod]
        public void GenerateAssignments_WithInvalidId_ReturnsError()
        {
            GroupRepository sut = new();

            AssignmentResult result = sut.GenerateAssignments(42);

            Assert.AreEqual("Group not found", result.ErrorMessage);
        }

        [TestMethod]
        public void GenerateAssignments_WithLessThanThreeUsers_ReturnsError()
        {
            GroupRepository sut = new();
            sut.Create(new()
            {
                Id = 42,
                Name = "Group"
            });

            AssignmentResult result = sut.GenerateAssignments(42);

            Assert.AreEqual($"Group Group must have at least three users", result.ErrorMessage);
        }

        [TestMethod]
        public void AddToGroup_WithValidItems_Success()
        {
            User u1;
            //GroupUser gu1;
            GroupRepository sut = new();
            UserRepository ust = new();
            Group group = new()
            {
                Id = 42,
                Name = "Group"
            };
            u1 = (new User { Id = 9, FirstName = "John", LastName = "Doe" });
            
            ust.Create(u1);

            sut.Create(group);

            sut.AddToGroup(group.Id, u1.Id);

            //Assert.AreEqual($"Group Group must have at least three users", result.ErrorMessage);
        }

        [TestMethod]
        public void MakeGroupUsers_WithValidItems_Success()
        {
            using DbContext dbContext = new DbContext();
            User u1;
            //GroupUser gu1;
            GroupRepository sut = new();
            UserRepository ust = new();
            Group group = new()
            {
                Id = 42,
                Name = "Group"
            };
            u1 = (new User { Id = 9, FirstName = "John", LastName = "Doe" });
            
            //dbContext.Add<User>(u1);
            //dbContext.Add<Group>(group);

            GroupUser gu = new GroupUser{
                Group = group,
                User = u1
            };

            dbContext.Add<GroupUser>(gu);

            dbContext.SaveChangesAsync();


            //dbContext.GroupUsers.Add(gu);


            sut.AddToGroup(group.Id, u1.Id);
        }

        [TestMethod]
        public void GenerateAssignments_WithValidGroup_CreatesAssignments()
        {
            using DbContext dbContext = new DbContext();
            User u1, u2, u3;
            //
            GroupRepository sut = new();
            UserRepository ust = new();
            Group group = new()
            {
                Id = 42,
                Name = "Group"
            };
            u1 = (new User { Id = 9, FirstName = "John", LastName = "Doe" });
            u2 = (new User { Id = 10, FirstName = "Jane", LastName = "Smith" });
            u3 = (new User { Id = 11, FirstName = "Bob", LastName = "Jones" });

            GroupUser gu1 = new GroupUser{
                Group = group,
                User = u1
            };
            GroupUser gu2 = new GroupUser{
                Group = group,
                User = u2
            };
            GroupUser gu3 = new GroupUser{
                Group = group,
                User = u3
            };

            dbContext.Add<GroupUser>(gu1);
            dbContext.Add<GroupUser>(gu2);
            dbContext.Add<GroupUser>(gu3);


            dbContext.SaveChangesAsync();

            Assignment a1 = new Assignment(u1, u2);
            GroupAssignment ga1 = new GroupAssignment{
                Assignment = a1,
                Group = group
            };

            //dbContext.Add<Assignment>(a1);
            //dbContext.Add<GroupAssignment>(ga1);
            //dbContext.SaveChangesAsync();


            AssignmentResult result = sut.GenerateAssignments(42);

            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(3, sut.GetItem(group.Id)!.Assignments.Count);
            Assert.AreEqual(3, sut.GetItem(group.Id)!.Assignments.Select(x => x.Giver.FirstName).Distinct().Count());
            Assert.AreEqual(3, sut.GetItem(group.Id)!.Assignments.Select(x => x.Receiver.FirstName).Distinct().Count());
            Assert.IsFalse(sut.GetItem(group.Id)!.Assignments.Any(x => x.Giver.FirstName == x.Receiver.FirstName));
        }
    }
}
