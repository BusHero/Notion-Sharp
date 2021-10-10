using System;

namespace Notion.Model
{
    public record User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Uri AvatarUrl { get; set; }

        public static T Copy<T>(User user) where T : User, new() => new()
        {
            Id = user.Id,
            Name = user.Name,
            AvatarUrl = user.AvatarUrl
        };

        public record Person : User
        {
            public string Email { get; set; }
        }

        public record Bot : User { }

    }

    public static class Users
    {
        public static T Cast<T>(this User user) where T : User, new() => new() 
        { 
            AvatarUrl = user.AvatarUrl, 
            Id = user.Id, 
            Name = user.Name 
        };
    }
}