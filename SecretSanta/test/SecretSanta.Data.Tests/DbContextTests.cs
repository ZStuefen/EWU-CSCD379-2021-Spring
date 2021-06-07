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
    }
}