using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace gudang_net_baru.Models.Master.Ekspedisi
{
    public class EkspedisiDto
    {
        [Required]
        public string? NamaEkspedisi { get; set; }
        [Required]
        public string? JenisLayanan { get; set; }
        [Required]
        public string? SlaPengiriman { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
    }
}
