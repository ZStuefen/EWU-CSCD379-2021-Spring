using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecretSanta.Data
{
    public class GroupAssignment
    {
        public int GroupId { get; set; }
        public Group Group { get; set; } = new();
        public int AssignmentId { get; set; }
        public Assignment Assignment { get; set; } = new();
    }
}
