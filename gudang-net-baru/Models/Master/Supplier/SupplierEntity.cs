using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace gudang_net_baru.Models.Master.Supplier
{
    public class SupplierEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? IdSupplier { get; set; } = default!;
        [AllowNull]
        public string? KodeSupplier { get; set; }
        [AllowNull]
        public string? NamaSupplier { get; set; }
        [AllowNull]
        public string? Alamat { get; set; }
        [AllowNull]
        public bool? Status { get; set; }
        [AllowNull]
        public string? TermOfDelivery { get; set; }
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
