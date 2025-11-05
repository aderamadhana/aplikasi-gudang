using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace gudang_net_baru.Models.Transaction.PurchaseOrder
{
    public class GoodReceiveEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string GrnId { get; set; } = default!;
        public string? GrnNo { get; set; } = default!;
        public string? PoId { get; set; } = default!;
        public string? PoNo { get; set; } = default!;
        public DateTime? TanggalTerima { get; set; }
        public string? UserId { get; set; } = default!;
        public string? StatusGr { get; set; }
        public string? LokasiId { get; set; }
        public string? LokasiName { get; set; }
        [AllowNull]
        public List<GoodReceiveDetailEntity> Details { get; set; } = new();
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

    public class GoodReceiveDetailEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string GrnDetailId { get; set; } = default!;
        public string? ItemId { get; set; }
        public string? ItemName { get; set; }
        public string? LotNo { get; set; }
        public int? QtyOrder { get; set; }
        public int? QtyReceived { get; set; }
        public string? StatusQC { get; set; }
        public string? Expiry { get; set; }
    }
}

