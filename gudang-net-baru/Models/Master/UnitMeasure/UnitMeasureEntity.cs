using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace gudang_net_baru.Models.Master.UnitMeasure
{
    public class UnitMeasureEntity
    {
        [Key]
        [NotNull]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string? IdUnitMeasure { get; set; } = default!;
        [AllowNull]
        public int? Ea { get; set; }
        [AllowNull]
        public int? Box { get; set; }
        [AllowNull]
        public int? Carton { get; set; }
        [AllowNull]
        public int? Pallet { get; set; }
        [AllowNull]
        public int? Conversion { get; set; }
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
