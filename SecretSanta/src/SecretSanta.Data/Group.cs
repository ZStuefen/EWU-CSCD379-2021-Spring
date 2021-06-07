using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecretSanta.Data
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";

        [NotMapped]
        public List<User> Users = new();
        public List<GroupUser> GroupUser = new();
        public List<GroupAssignment> GroupAssignment = new();
        public List<Assignment> Assignments { get; } = new();

        public override string ToString()
        {
            return $"{Id} {Name}";
        }
    }
}
