using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace gudang_net_baru.Models.Master.Sku
{
    public class SkuEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? IdSku { get; set; } = default!;
        [AllowNull]
        public string? KodeSku { get; set; }
        [AllowNull]
        public string? NamaBarang { get; set; }
        [AllowNull]
        public string? Deskripsi { get; set; }
        [AllowNull]
        public string? UnitMeasureId { get; set; }
        [AllowNull]
        public string? UnitMeasureName { get; set; }
        [AllowNull]
        public int? Dimensi { get; set; }
        [AllowNull]
        public string? JenisBarang { get; set; }
        [AllowNull]
        public string? LotTracking { get; set; }
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
