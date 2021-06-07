using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SecretSanta.Data.Tests
{
    [TestClass]
    public class DbContextTests
    {
        [TestMethod]
        async public Task Add_NewGroup_Success()
        {
            Group @group;
            using DbContext dbContext = new DbContext();
            string namePrefix = $"{nameof(DbContextTests)} {nameof(Add_NewGroup_Success)}";
            async Task RemoveExistingTestGroupsAsync()
            {
                // remove code here
                IQueryable<Group>? groups = dbContext.Groups.Where(
                    item => item.Name.StartsWith(namePrefix));
                dbContext.Groups.RemoveRange(groups);
                await dbContext.SaveChangesAsync();
            }

            try
            {
                int countBefore = dbContext.Groups.Count();
                // remove code here
                await RemoveExistingTestGroupsAsync();

              
                @group = new Group() { Name = $"{namePrefix} " + Guid.NewGuid().ToString() };
                int id = @group.Id;
                await dbContext.Groups.AddAsync(@group);
                Assert.AreEqual<int>(0, @group.Id);
                await dbContext.SaveChangesAsync();
                Assert.AreNotEqual<int>(id, @group.Id);
                Assert.AreEqual(countBefore + 1, dbContext.Groups.Count());
            }
            finally
            {
                await RemoveExistingTestGroupsAsync();
            }
        }
        
        [TestMethod]
        async public Task Add_NewGroup_UsingTransactions()
        {
            Group @group;
            User @user1, @user2, @user3;
            Assignment @a1, a2, a3;
            GroupUser @gu1, @gu2, @gu3;
            GroupAssignment @ga1, @ga2, @ga3;
            using DbContext dbContext = new DbContext();
            string namePrefix = $"{nameof(DbContextTests)} {nameof(Add_NewGroup_Success)}";
            async Task RemoveExistingTestGroupsAsync()
            {
                // remove code here
                IQueryable<Group>? groups = dbContext.Groups.Where(
                    item => item.Name.StartsWith(namePrefix));
                dbContext.Groups.RemoveRange(groups);
                await dbContext.SaveChangesAsync();
            }

            try
            {
                int countBefore = dbContext.Groups.Count();
                // remove code here
                await RemoveExistingTestGroupsAsync();

              
                @group = new Group() { Name = $"{namePrefix} " + Guid.NewGuid().ToString() };
                @user1 = new User() { FirstName = "OneFirst", LastName = "OneLast"};
                @user2 = new User() { FirstName = "TwoFirst", LastName = "TwoLast"};
                @user3 = new User() { FirstName = "ThreeFirst", LastName = "ThreeLast"};
                int id = @group.Id;
                dbContext.Groups.Add(@group);

                dbContext.Users.Add(@user1);
                dbContext.Users.Add(@user2);
                dbContext.Users.Add(@user3);


                @group.Users.Add(@user1);
                @group.Users.Add(@user2);
                @group.Users.Add(@user3);

                @gu1 = new GroupUser();
                @gu1.User = @user1;
                @gu1.Group = @group;
                @gu2 = new GroupUser();
                @gu2.User = @user2;
                @gu2.Group = @group;
                @gu3 = new GroupUser();
                @gu3.User = @user3;
                @gu3.Group = @group;

                dbContext.GroupUsers.Add(@gu1);
                dbContext.GroupUsers.Add(@gu2);
                dbContext.GroupUsers.Add(@gu3);

                @a1 = new Assignment(@user1, @user2);
                @a2 = new Assignment(@user2, @user3);
                @a3 = new Assignment(@user3, @user1);

                @group.Assignments.Add(@a1);
                @group.Assignments.Add(@a2);
                @group.Assignments.Add(@a3);

                @ga1 = new GroupAssignment();
                @ga1.Assignment = @a1;
                @ga1.Group = @group;
                @ga2 = new GroupAssignment();
                @ga2.Assignment = @a2;
                @ga2.Group = @group;
                @ga3 = new GroupAssignment();
                @ga3.Assignment = @a3;
                @ga3.Group = @group;

                @group.GroupAssignment.Add(@ga1);
                @group.GroupAssignment.Add(@ga2);
                @group.GroupAssignment.Add(@ga3);

                //dbContext.GroupAssignments.Add

                Assert.AreEqual<int>(0, @group.Id);
                await dbContext.SaveChangesAsync();
                Assert.AreNotEqual<int>(id, @group.Id);
                Assert.AreEqual(countBefore + 1, dbContext.Groups.Count());
            }
            finally
            {
                await RemoveExistingTestGroupsAsync();
            }
        }
    }
}