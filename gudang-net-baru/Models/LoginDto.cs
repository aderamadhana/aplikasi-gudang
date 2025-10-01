using System.ComponentModel.DataAnnotations;

namespace gudang_net_baru.Models
{
    public class LoginDto
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public Boolean RememberMe { get; set; } = false;
    }
}
