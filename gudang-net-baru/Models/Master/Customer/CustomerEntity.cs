using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace gudang_net_baru.Models.Master.Customer
{
    public class CustomerEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? IdCustomer { get; set; } = default!;
        [AllowNull]
        public string? NamaCustomer { get; set; }
        [AllowNull]
        public string? Alamat { get; set; }
        [AllowNull]
        public string? JenisCustomer { get; set; }
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
