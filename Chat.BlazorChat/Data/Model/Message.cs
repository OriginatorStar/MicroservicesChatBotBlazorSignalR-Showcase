using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Chat.BlazorChat.Data.Model
{
    public class Message
    {
        public Message()
        {
            Timestamp = DateTime.UtcNow;
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Content { get; set; }

        /// <summary>
        /// Created timestamp
        /// </summary>
        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string Username { get; set; }

        [NotMapped]
        public string ShortUsername { get { return this.Username.Contains("@") ? this.Username.Substring(0, this.Username.IndexOf("@")) : this.Username; } }

        [Required]
        [ForeignKey("ChatRoom")]
        public Guid ChatRoomId { get; set; }

        public virtual ChatRoom ChatRoom { get; set; }
    }
}
