using System.Diagnostics.CodeAnalysis;

namespace gudang_net_baru.Models.Transaction.PurchaseOrder
{
    public class PurchaseOrderDto
    {
        public string SupplierId { get; set; } = string.Empty;
        public string? SupplierName { get; set; }
        public string? PoNumber { get; set; }
        public DateOnly TanggalPo { get; set; }
        public string? StatusPo { get; set; }
        public int? TotalQty { get; set; }
        public string? Keterangan { get; set; }
        public List<PurchaseOrderDetailDto> Items { get; set; } = new();
        public bool? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
    }

    public class PurchaseOrderDetailDto
    {
        public string? ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public int? QtyOrder { get; set; }
        public int? QtyReceived { get; set; }
        public string? UnitMeasureName { get; set; }
        public string? KeteranganNotes { get; set; }
    }
}
