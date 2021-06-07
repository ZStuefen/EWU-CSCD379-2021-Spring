using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using SecretSanta.Data;

namespace SecretSanta.Business.Tests
{
    [TestClass]
    public class GiftRepositoryTests
    {
        [TestCleanup]
        async public Task Clear_Database()
        {
            using DbContext dbContext = new DbContext();
            IQueryable<Gift>? gifts = dbContext.Gifts.Where(
                item => item.Title.StartsWith(""));
            dbContext.Gifts.RemoveRange(gifts);
            await dbContext.SaveChangesAsync();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_NullItem_ThrowsArgumentException()
        {
            GiftRepository sut = new();

            sut.Create(null!);
        }

        [TestMethod]
        public void Create_WithItem_CanGetItem()
        {
            GiftRepository sut = new();
            Gift gift = new()
            {
                Id = 42
            };

            Gift createdGift = sut.Create(gift);

            Gift? retrievedGift = sut.GetItem(createdGift.Id);
            Assert.AreEqual(gift.Id, retrievedGift!.Id);
            Assert.AreEqual(gift.Title, retrievedGift.Title);
            Assert.AreEqual(gift.Description, retrievedGift.Description);
        }

        [TestMethod]
        public void GetItem_WithBadId_ReturnsNull()
        {
            GiftRepository sut = new();

            Gift? gift = sut.GetItem(-1);

            Assert.IsNull(gift);
        }

        [TestMethod]
        public void GetItem_WithValidId_ReturnsGift()
        {
            GiftRepository sut = new();
            Gift temp = new Gift();
            temp.Id = 42; temp.Title = "First"; temp.Description = "Last";
            /*
            sut.Create(new() 
            { 
                Id = 42,
                Title = "First",
                Description = "Last"
            });
            */
            sut.Create(temp);

            Gift? gift = sut.GetItem(42);

            Assert.AreEqual(42, gift?.Id);
            Assert.AreEqual("First", gift!.Title);
            Assert.AreEqual("Last", gift.Description);
        }

        [TestMethod]
        public void List_WithGifts_ReturnsAllGift()
        {
            using DbContext dbContext = new DbContext();
            GiftRepository sut = new();
            sut.Create(new()
            {
                Id = 42,
                Title = "First",
                Description = "Last"
            });

            ICollection<Gift> gifts = sut.List();

            Assert.AreEqual(dbContext.Gifts.Count(), gifts.Count);
            foreach(var mockGift in dbContext.Gifts)
            {
                Assert.IsNotNull(gifts.SingleOrDefault(x => x.Title == mockGift.Title && x.Description == mockGift.Description));
            }
        }

        [TestMethod]
        [DataRow(-1, false)]
        [DataRow(42, true)]
        public void Remove_WithInvalidId_ReturnsTrue(int id, bool expected)
        {
            GiftRepository sut = new();
            sut.Create(new()
            {
                Id = 42,
                Title = "First",
                Description = "Last"
            });

            Assert.AreEqual(expected, sut.Remove(id));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Save_NullItem_ThrowsArgumentException()
        {
            GiftRepository sut = new();

            sut.Save(null!);
        }

        [TestMethod]
        public void Save_WithValidItem_SavesItem()
        {
            GiftRepository sut = new();

            sut.Save(new Gift() { Id = 42 });

            Assert.AreEqual(42, sut.GetItem(42)?.Id);
        }
    }
}
