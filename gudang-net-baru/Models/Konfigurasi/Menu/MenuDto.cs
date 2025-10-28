using System.ComponentModel.DataAnnotations;

namespace gudang_net_baru.Models.Konfigurasi.Menu
{
    public class MenuDto
    {
        public string? ControllerName { get; set; } = string.Empty;
        public string? ControllerFunction { get; set; } = string.Empty;
        public string? MenuIcon { get; set; } = string.Empty;
        [Required]
        public string? MenuName { get; set; } = string.Empty;
        [Required]
        public string? MenuType { get; set; } = string.Empty;
        public string? ParentId { get; set; } = string.Empty;
        [Required]
        public string? RoleId { get; set; } = string.Empty;
        public bool? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; } = string.Empty;
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; } = string.Empty;
        public int Urutan { get; set; }
        public List<MenuDto> Children { get; set; } = new();
        public string? IdMenu { get; set; } = string.Empty;
    }
}
