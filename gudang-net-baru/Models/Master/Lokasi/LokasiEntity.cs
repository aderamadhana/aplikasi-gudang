using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace gudang_net_baru.Models.Master.Lokasi
{
    public class LokasiEntity
    {
        [Key]
        [NotNull]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string? IdLokasi { get; set; } = default!;
        [AllowNull]
        public string? Warehouse { get; set; }
        [AllowNull]
        public string? TipeLokasi { get; set; }
        [AllowNull]
        public int? Kapasitas { get; set; }
        [AllowNull]
        public bool? Status { get; set; }
        [AllowNull]
        public DateTime? CreatedAt { get; set; }
        [AllowNull]
        public string? CreatedBy { get; set; }
        [AllowNull]
        public DateTime? UpdatedAt { get; set; }
        [AllowNull]
        public string? UpdatedBy { get; set; }
        [AllowNull]
        public DateTime? DeletedAt { get; set; }
        [AllowNull]
        public string? DeletedBy { get; set; }
    }
}
