using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace gudang_net_baru.Models.Transaction.PurchaseOrder
{
    public class PurchaseOrderEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string PurchaseOrderId { get; set; } = default!;
        public string? SupplierId { get; set; } = default!;
        public string? SupplierName { get; set; } = default!;
        public string? PoNumber { get; set; } = default!;
        public DateTime? TanggalPo { get; set; }
        public string? StatusPo { get; set; }
        [AllowNull]
        public int? TotalQty { get; set; }
        public string? Keterangan { get; set; }
        [AllowNull]
        public List<PurchaseOrderDetailEntity> Details { get; set; } = new();
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

    public class PurchaseOrderDetailEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string PurchaseOrderDetailId { get; set; } = default!;

        public string? ItemId { get; set; }
        public string? ItemName { get; set; }
        public int? QtyOrder { get; set; }
        public int? QtyReceived { get; set; }
        public string? UnitMeasureName { get; set; }
        public string? Keterangan { get; set; }
    } 
}

