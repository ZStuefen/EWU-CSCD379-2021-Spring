using System.Collections.Generic;
using SecretSanta.Data;

namespace SecretSanta.Business
{
    public class GiftRepository : IGiftRepository
    {
        public Gift Create(Gift item)
        {
            if (item is null)
            {
                throw new System.ArgumentNullException(nameof(item));
            }
            using DbContext dbContext = new DbContext();
            dbContext.Gifts.Add(item);
            dbContext.SaveChangesAsync();
            //MockData.Gifts[item.Id] = item;
            return item;
        }

        public Gift? GetItem(int id)
        {
            using DbContext dbContext = new DbContext();
            Gift gift = dbContext.Gifts.Find(id);
            //if (MockData.Gifts.TryGetValue(id, out Gift? gift))
            //{
            //    return gift;
            //}
            //return null;
            return gift;
        }

        public ICollection<Gift> List()
        {
            //return MockData.Gifts.Values;
            using DbContext dbContext = new DbContext();
            List<Gift> giftList = new List<Gift>();
            foreach (var gift in dbContext.Gifts)
            {
                giftList.Add(gift);
            }
            return giftList;
        }

        public bool Remove(int id)
        {
            try
            {
                using DbContext dbContext = new DbContext();
                Gift item = dbContext.Gifts.Find(id);
                dbContext.Gifts.Remove(item);
                dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Save(Gift item)
        {
            if (item is null)
            {
                throw new System.ArgumentNullException(nameof(item));
            }
            using DbContext dbContext = new DbContext();

            //MockData.Gifts[item.Id] = item;
            Gift temp = dbContext.Gifts.Find(item.Id);
            if (temp is null)
            {
                Create(item);
            }
            else
            {
                dbContext.Gifts.Remove(dbContext.Gifts.Find(item.Id));
                dbContext.Gifts.Add(item);
            }
            dbContext.SaveChangesAsync();
        }
    }
}
