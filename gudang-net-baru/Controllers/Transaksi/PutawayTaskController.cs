using gudang_net_baru.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace gudang_net_baru.Controllers.Transaksi
{
    public class PutawayTaskController : Controller
    {
        ApplicationDbContext context;
        public PutawayTaskController(ApplicationDbContext context ) { 
            this.context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            ViewBagLoad();
            return View();
        }
        private void ViewBagLoad()
        {
            var good_receive = context.GoodReceive
                .Where(x => x.DeletedAt == null)
                .Where(x => x.StatusGr == "Posted")
                .Include(x => x.Details)
                .Join(context.PurchaseOrder,
                    gr => gr.PoId,
                    po => po.PurchaseOrderId,
                    (gr, po) => new { gr, po })
                .Join(context.Users,
                    x => x.gr.UserId,
                    u => u.Id,
                    (x, u) => new { x.gr, x.po, u })
                .OrderBy(x => x.gr.GrnNo)
                .AsEnumerable()
                .Select(x =>
                {
                    x.gr.Details = x.gr.Details.Where(d => d.StatusQC != "reject").ToList();
                    return new
                    {
                        GrnId = x.gr.GrnId,
                        GrnNo = x.gr.GrnNo,
                        PoNo = x.gr.PoNo,
                        PoId = x.po.PurchaseOrderId,
                        UserId = x.gr.UserId,
                        SupplierName = x.po.SupplierName,
                        UserName = x.u.UserName,
                        TanggalTerima = x.gr.TanggalTerima,
                        LokasiName = x.gr.LokasiName,
                        StatusGr = x.gr.StatusGr,
                        Details = x.gr.Details
                    };
                })
                .ToList();

            ViewBag.GoodReceive = good_receive;
        }
    }
}
