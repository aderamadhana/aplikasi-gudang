using Microsoft.AspNetCore.Mvc;

namespace gudang_net_baru.Controllers.Transaksi
{
    public class PurchaseOrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }
    }
}
