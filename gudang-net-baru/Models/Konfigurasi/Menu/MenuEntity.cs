using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace gudang_net_baru.Models.Konfigurasi.Menu
{
    public class MenuEntity
    {
        [Key]
        [NotNull]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string IdMenu { get; set; }
        [AllowNull]
        public string MenuName { get; set; }
        [AllowNull]
        public string ControllerName { get; set; }
        [AllowNull]
        public string ControllerFunction { get; set; }
        [AllowNull]
        public string MenuType { get; set; }
        [AllowNull]
        public string ParentId {  get; set; }
        [AllowNull]
        public string MenuIcon { get; set; }
        [AllowNull]
        public string RoleId { get; set; }
        [AllowNull]
        public bool Status { get; set; }
        [AllowNull]
        public DateTime CreatedAt { get; set; }
        [AllowNull]
        public string CreatedBy { get; set; }
        [AllowNull]
        public DateTime UpdatedAt { get; set; }
        [AllowNull]
        public string UpdatedBy { get; set; }
        [AllowNull]
        public DateTime DeletedAt { get; set; }
        [AllowNull]
        public string DeletedBy { get; set; }
        [AllowNull]
        public int Urutan { get; set; }
    }
}
