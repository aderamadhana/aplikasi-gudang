using Microsoft.AspNetCore.Mvc;

namespace gudang_net_baru.Controllers.Transaksi
{
    public class SalesOrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
