using gudang_net_baru.Models;
using gudang_net_baru.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace gudang_net_baru.Controllers.Transaksi
{
    public class PurchaseOrderController : Controller
    {
        private readonly ApplicationDbContext context;
        public PurchaseOrderController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            var supplier = context.MasterSupplier
                .Where(r => r.Status == true)
                .OrderBy(r => r.NamaSupplier)
                .ToList();

            var sku = context.MasterSku
                .Where(r => r.Status == true)
                .OrderBy(r => r.NamaBarang)
                .ToList();

            ViewBag.ListSupplier = supplier;
            ViewBag.ListSku = sku;

            return View();
        }
    }
}
