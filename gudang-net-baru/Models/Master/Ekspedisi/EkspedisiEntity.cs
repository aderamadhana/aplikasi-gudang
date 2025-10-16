using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace gudang_net_baru.Models.Master.Ekspedisi
{
    public class EkspedisiEntity
    {
        [Key]
        [NotNull]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string? IdEkspedisi { get; set; } = default!;
        [AllowNull]
        public string? NamaEkspedisi { get; set; }
        [AllowNull]
        public string? JenisLayanan { get; set; }
        [AllowNull]
        public string? SlaPengiriman { get; set; }
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
