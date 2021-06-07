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
            using DbContext dbContext = new DbContext();
            dbContext.Add<Group>(item);
            foreach (User user in item.Users)
            {
                AddToGroup(item.Id, user.Id);
            }
            dbContext.SaveChangesAsync();
            return item;
        }

        public Group? GetItem(int id)
        {
            using DbContext dbContext = new DbContext();
            Group group = dbContext.Groups.Find(id);
            List<GroupUser> groupUserList = new List<GroupUser>();
            foreach (GroupUser groupUser in dbContext.GroupUsers)
            {
                if (groupUser.GroupId == group.Id)
                {
                    //groupUserList.Add(groupUser);
                    group.Users.Add(dbContext.Users.Find(groupUser.UserId));
                }
            }
            List<GroupAssignment> groupAssignments = new List<GroupAssignment>();
            foreach (GroupAssignment groupAssignment in dbContext.GroupAssignments)
            {
                if (groupAssignment.GroupId == group.Id)
                {
                    //groupUserList.Add(groupUser);
                    group.Assignments.Add(
                        new Assignment(dbContext.Users.Find(dbContext.Assignments.Find(groupAssignment.AssignmentId).GiverId), dbContext.Users.Find(dbContext.Assignments.Find(groupAssignment.AssignmentId).ReceiverId)));
                }
            }
            /*
            if (MockData.Groups.TryGetValue(id, out Group? user))
            {
                return user;
            }
            return null;
            */
            return group;
        }

        public ICollection<Group> List()
        {
            //return MockData.Groups.Values;
            using DbContext dbContext = new DbContext();
            List<Group> groupList = new List<Group>();
            foreach (var group in dbContext.Groups)
            {
                groupList.Add(group);
            }
            return groupList;
        }

        public bool Remove(int id)
        {
            try
            {
                using DbContext dbContext = new DbContext();
                Group item = dbContext.Groups.Find(id);
                dbContext.Groups.Remove(item);
                dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Save(Group item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            using DbContext dbContext = new DbContext();

            //MockData.Groups[item.Id] = item;
            Group temp = dbContext.Groups.Find(item.Id);
            if (temp is null)
            {
                Create(item);
            }
            else
            {
                dbContext.Groups.Remove(dbContext.Groups.Find(item.Id));
                Create(item);
            }
            dbContext.SaveChangesAsync();
        }
        
        public AssignmentResult GenerateAssignments(int groupId)
        {
            Assignment assignment;
            GroupAssignment groupAssignment;
            using DbContext dbContext = new DbContext();

            //if (!dbContext.Groups.TryGetValue(groupId, out Group? group))
            Group group = GetItem(groupId)!;
            if (group is null)
            {
                return AssignmentResult.Error("Group not found");
            }

            Random random = new();
            var groupUsers = new List<User>(group.Users);

            if (groupUsers.Count < 3)
            {
                return AssignmentResult.Error($"Group {group.Name} must have at least three users");
            }

            var users = new List<User>();
            //Put the users in a random order
            while(groupUsers.Count > 0)
            {
                int index = random.Next(groupUsers.Count);
                users.Add(groupUsers[index]);
                groupUsers.RemoveAt(index);
            }

            //The assignments are created by linking the current user to the next user.
            group.Assignments.Clear();
            for(int i = 0; i < users.Count; i++)
            {
                int endIndex = (i + 1) % users.Count;
                assignment = new Assignment(dbContext.Users.Find(users[i].Id), dbContext.Users.Find(users[endIndex].Id));

                //group.Assignments.Add(assignment);
                //dbContext.Assignments.Add(assignment);

                groupAssignment = ( new GroupAssignment{
                    Assignment = assignment,
                    Group = dbContext.Groups.Find(group.Id)
                });
                
                dbContext.AddAsync<Assignment>(assignment);
                //group.Assignments.Add(assignment);
                //group.GroupAssignment.Add(groupAssignment[i]);
                dbContext.AddAsync<GroupAssignment>(groupAssignment);
                dbContext.SaveChangesAsync();
            }
            //dbContext.Add<Group>(group);
            //dbContext.AddRange(groupAssignment);
            dbContext.SaveChangesAsync();
            return AssignmentResult.Success();
        }

        public void AddToGroup(int groupId, int userId)
        {
            using DbContext dbContext = new DbContext();
            
            Group group = dbContext.Groups.Find(groupId);
            User user = dbContext.Users.Find(userId);
            if (group is null)
            {
                throw new ArgumentNullException(nameof(group));
            }
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            
            GroupUser groupUser = new GroupUser();
            groupUser.User = user;
            groupUser.UserId = user.Id;
            groupUser.Group = group;
            groupUser.GroupId = group.Id;

            dbContext.Add<GroupUser>(groupUser);
            dbContext.SaveChanges();
        }

        public void RemoveFromGroup(int groupId, int userId)
        {
            using DbContext dbContext = new DbContext();

            dbContext.GroupUsers.Remove(dbContext.GroupUsers.Find(groupId, userId));

            dbContext.SaveChanges();
        }
    }
}
