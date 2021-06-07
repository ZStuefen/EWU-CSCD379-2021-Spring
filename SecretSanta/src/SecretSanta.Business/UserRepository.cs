using System.Collections.Generic;
using SecretSanta.Data;

namespace SecretSanta.Business
{
    public class UserRepository : IUserRepository
    {
        public User Create(User item)
        {
            if (item is null)
            {
                throw new System.ArgumentNullException(nameof(item));
            }
            using DbContext dbContext = new DbContext();
            dbContext.Users.Add(item);
            dbContext.SaveChangesAsync();
            //MockData.Users[item.Id] = item;
            return item;
        }

        public User? GetItem(int id)
        {
            using DbContext dbContext = new DbContext();
            User user = dbContext.Users.Find(id);
            //if (MockData.Users.TryGetValue(id, out User? user))
            //{
            //    return user;
            //}
            //return null;
            return user;
        }

        public ICollection<User> List()
        {
            //return MockData.Users.Values;
            using DbContext dbContext = new DbContext();
            List<User> userList = new List<User>();
            foreach (var user in dbContext.Users)
            {
                userList.Add(user);
            }
            return userList;
        }

        public bool Remove(int id)
        {
            try
            {
                using DbContext dbContext = new DbContext();
                User item = dbContext.Users.Find(id);
                dbContext.Users.Remove(item);
                dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Save(User item)
        {
            if (item is null)
            {
                throw new System.ArgumentNullException(nameof(item));
            }
            using DbContext dbContext = new DbContext();

            //MockData.Users[item.Id] = item;
            User temp = dbContext.Users.Find(item.Id);
            if (temp is null)
            {
                Create(item);
            }
            else
            {
                dbContext.Users.Remove(dbContext.Users.Find(item.Id));
                dbContext.Users.Add(item);
            }
            dbContext.SaveChangesAsync();
        }
        /*
        private User CopyUser(User item)
        {
            if (item is null)
            {
                return new User();
            }
            User newUser = new User
            {
                Id = item.Id,
                FirstName = item.FirstName,
                LastName = item.LastName
            };
            foreach (Group group in item.Groups)
            {
                newUser.Groups.Add(group);
            }
            return newUser;
        }
        */
    }
}
