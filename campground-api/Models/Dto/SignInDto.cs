using System.ComponentModel.DataAnnotations;

namespace campground_api.Models.Dto
{
    public class SignInDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
    }
}
