using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace gudang_net_baru.Models.Master.UnitMeasure
{
    public class UnitMeasureDto
    {
        public string? IdUnitMeasure { get; set; } = string.Empty;
        public string? UnitMeasureName { get; set; }
        public string? Deskripsi { get; set; }
        public string? Conversion { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
    }
}
