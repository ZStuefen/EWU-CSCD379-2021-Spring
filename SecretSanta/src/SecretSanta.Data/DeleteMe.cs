using System.Collections.Generic;

namespace SecretSanta.Data
{
    public static class DeleteMe
    {
        public static List<User> Users { get; } = new()
        {
            new User() { Id = 1, FirstName = "Inigo", LastName = "Montoya" },
            new User() { Id = 2, FirstName = "Princess", LastName = "Buttercup" },
            new User() { Id = 3, FirstName = "Prince", LastName = "Humperdink" },
            new User() { Id = 4, FirstName = "Count", LastName = "Rugen" },
            new User() { Id = 5, FirstName = "Miracle", LastName = "Max" },
        };
    }
}