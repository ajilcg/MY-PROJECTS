using System.ComponentModel.DataAnnotations;

namespace BBPSApi.Model
{
    public class LoginRequestModel
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
