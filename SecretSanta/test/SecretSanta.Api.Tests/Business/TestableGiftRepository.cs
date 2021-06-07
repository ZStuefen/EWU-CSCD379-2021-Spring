using System.Collections.Generic;
using SecretSanta.Business;
using SecretSanta.Data;

namespace SecretSanta.Api.Tests.Business
{
    internal class TestableGiftRepository : IGiftRepository
    {
        private Dictionary<int, Gift> Gifts { get; } = new();

        public Gift Create(Gift item)
        {
            Gifts.Add(item.Id, item);
            return item;
        }

        public Gift? GetItem(int id)
        {
            Gifts.TryGetValue(id, out Gift? rv);
            return rv;
        }

        public ICollection<Gift> List() => Gifts.Values;

        public bool Remove(int id) => Gifts.Remove(id);

        public void Save(Gift item) => Gifts[item.Id] = item;
    }
}
