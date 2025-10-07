using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace gudang_net_baru.Models
{
    public class ApplicationRole
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; } = string.Empty;
    }
}
