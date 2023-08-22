using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chat.BlazorChat.Data.Model
{
    public class User : IdentityUser
    {
        public User()
        {
            Messages = new HashSet<Message>();
        }

        public virtual ICollection<Message> Messages { get; set; }
    }
}
