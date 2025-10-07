using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace gudang_net_baru.Models
{
    public class RoleDto
    {
        [Required(ErrorMessage="Nama Role wajib di isi")]
        [RegularExpression("^[A-Z0-9]+$", ErrorMessage = "Hanya A–Z dan 0–9, tanpa spasi/karakter spesial.")]
        public string Name { get; set; }
        public string NormalizedName { get; set; } = string.Empty;
        public string Id { get; set; }
    }
}
