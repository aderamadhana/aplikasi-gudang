using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace gudang_net_baru.Models.Master.Customer
{
    public class CustomerDto
    {
        [Required]
        public string? NamaCustomer { get; set; }
        [Required]
        public string? Alamat { get; set; }
        [Required]
        public string? JenisCustomer { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
    }
}
