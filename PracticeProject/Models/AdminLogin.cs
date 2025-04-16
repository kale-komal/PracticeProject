using System.ComponentModel.DataAnnotations;

namespace PracticeProject.Models
{
    public class AdminLogin
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(100)]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}

