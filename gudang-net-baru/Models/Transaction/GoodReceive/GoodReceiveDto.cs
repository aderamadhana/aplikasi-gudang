using gudang_net_baru.Models.Transaction.PurchaseOrder;
using System.Diagnostics.CodeAnalysis;

namespace gudang_net_baru.Models.Transaction.GoodReceive
{
    public class GoodReceiveDto
    {
        public string? GrnNo { get; set; } = default!;
        public string? PoId { get; set; } = default!;
        public string? PoNo { get; set; } = default!;
        public DateTime? TanggalTerima { get; set; }
        public string? UserId { get; set; } = default!;
        public string? StatusGr { get; set; }
        public string? LokasiId { get; set; }
        public string? LokasiName { get; set; }
        public List<GoodReceiveDetailDto> Items { get; set; } = new();
        public bool? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
    }

    public class GoodReceiveDetailDto
    {
        public string? ItemId { get; set; }
        public string? ItemName { get; set; }
        public string? LotNo { get; set; }
        public int? QtyOrder { get; set; }
        public int? QtyReceived { get; set; }
        public string? StatusQC { get; set; }
        public DateTime? Expiry { get; set; }
    }
}
