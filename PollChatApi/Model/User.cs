using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PollChatApi.Model
{
    public class User 
    {
      
        public string Id { get; set; }

        public string UserId { get; set; }

        //public bool? Banned { get; set; }

        public string ProfilePicture { get; set; }

        public ICollection<MainThread>? FavoriteThreads { get; set; }

        public ICollection<Subject>? FavoriteSubjects { get; set; }

        public ICollection<Warning>? Warnings { get; set; }
    }
}
