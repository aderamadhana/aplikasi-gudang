using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace gudang_net_baru.Models.Master.UnitMeasure
{
    public class UnitMeasureDto
    {
        public string? IdUnitMeasure { get; set; } = string.Empty;
        public string? UnitMeasureName { get; set; }
        public int? Ea { get; set; }
        public int? Box { get; set; }
        public int? Carton { get; set; }
        public int? Pallet { get; set; }
        public int? Conversion { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
    }
}
