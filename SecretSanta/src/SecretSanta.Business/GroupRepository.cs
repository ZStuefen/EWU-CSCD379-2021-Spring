using System;
using System.Collections.Generic;
using SecretSanta.Data;

namespace SecretSanta.Business
{
    public class GroupRepository : IGroupRepository
    {
        public Group Create(Group item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            MockData.Groups[item.Id] = item;
            return item;
        }

        public Group? GetItem(int id)
        {
            if (MockData.Groups.TryGetValue(id, out Group? user))
            {
                return user;
            }
            return null;
        }

        public ICollection<Group> List()
        {
            return MockData.Groups.Values;
        }

        public bool Remove(int id)
        {
            return MockData.Groups.Remove(id);
        }

        public void Save(Group item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            MockData.Groups[item.Id] = item;
        }

        public AssignmentResult GenerateAssignments(int id)
        {
            bool res = MockData.Groups.TryGetValue(id, out Group? group);
            if (res == false || group is null || group.Name is null || group.Name.Length == 0)
            {
                return AssignmentResult.Error("Group not found");
            }
            if (group.Users.Count <= 2)
            {
                return AssignmentResult.Error("Group " + group.Name + " must have at least three users");
            }
            List<User> users = group.Users;
            group.Assignments = new List<Assignment>();
            for (int x = 0; x < group.Users.Count; x++)
            {
                group.Assignments.Add(new Assignment(users[x], users[(x + 1) % users.Count]));
            }
            return AssignmentResult.Success();
        }
    }
}
