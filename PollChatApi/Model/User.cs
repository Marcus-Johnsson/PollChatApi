using Microsoft.AspNetCore.Identity;
using PollChatApi.Models;
using SubjectWars.Models;

namespace PollChatApi.Model
{
    public class User : IdentityUser
    {
    

        public bool Banned { get; set; }

        public ICollection<MainThread>? FavoriteThreads { get; set; }
        public ICollection<SubCategory>? FavoriteSubcategories { get; set; }
        public ICollection<Subject>? FavoriteSubjects { get; set; }

        public ICollection<Warning>? Warnings { get; set; }
    }
}
