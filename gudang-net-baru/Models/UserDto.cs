using System.ComponentModel.DataAnnotations;

namespace gudang_net_baru.Models
{
    public class UserDto
    {
        [Required(ErrorMessage = "Nama Depan wajib diisi!"), MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Nama Belakang wajib diisi!")]
        public string LastName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Email wajib diisi!")]
        [EmailAddress(ErrorMessage = "Email tidak valid!")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Username wajib diisi!")]
        public string UserName {  get; set; } = string.Empty;
        [Required(ErrorMessage = "Password wajib diisi!")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Konfirmasi Password wajib diisi!")]
        [Compare("Password", ErrorMessage = "Konfirmasi Password dan Password tidak sesuai!")]
        public string ConfirmPassword { get; set; } = string.Empty;
        public bool Status { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int DeletedBy { get; set; }
        public DateTime DeletedAt { get; set; }
        [Required(ErrorMessage = "Role wajib diisi!")]
        public List<string> SelectedRoleIds { get; set; }
        public List<string> Selected { get; set; } = new();
        public object Id { get; internal set; } = string.Empty;
    }
}
