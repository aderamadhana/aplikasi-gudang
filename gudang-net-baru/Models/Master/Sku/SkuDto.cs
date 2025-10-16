using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace gudang_net_baru.Models.Master.Sku
{
    public class SkuDto
    {
        [Required]
        public string? KodeSku { get; set; }
        [Required]
        public string? NamaBarang { get; set; }
        [Required]
        public string? Deskripsi { get; set; }
        [Required]
        public string? UnitMeasureId { get; set; }
        [Required]
        public string? UnitMeasureName { get; set; }
        [Required]
        public int? Dimensi { get; set; }
        [Required]
        public string? JenisBarang { get; set; }
        [Required]
        public string? LotTracking { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
    }
}
