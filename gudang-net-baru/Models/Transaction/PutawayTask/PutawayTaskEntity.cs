using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace gudang_net_baru.Models.Transaction.PutawayTask
{
    public class PutawayTaskEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string PutawayId { get; set; } = default!;
        public string? PutawayNumber { get; set; } = default!;
        public string? GoodReceiveId { get; set; } = default!;
        [AllowNull]
        public List<PutawayTaskDetailEntity> Details { get; set; } = new();
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

    public class PutawayTaskDetailEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string PutawayDetailId { get; set; } = default!;
        public string? PutawayId { get; set; }
        public string? GrDetailId { get; set; }
        public string? ItemId { get; set; }
        public string? ItemName { get; set; }
        public string? LotNo { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int? QtyToPutaway { get; set; }
        public string? FromLocationId { get; set; }
        public string? ToLocationId { get; set; }
        public string? Catatan { get; set; }
        public string? Status { get; set; }
    }
}

