using gudang_net_baru.Models.Master.Customer;
using gudang_net_baru.Models.Master.Ekspedisi;
using gudang_net_baru.Services;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace gudang_net_baru.Controllers.Master
{
    public class EkspedisiController : Controller
    {
        private readonly ApplicationDbContext context;
        public EkspedisiController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult getEkspedisi(bool status)
        {

            // DataTables params
            var draw = Request.Form["draw"].FirstOrDefault();
            int start = int.Parse(Request.Form["start"].FirstOrDefault() ?? "0");
            int length = int.Parse(Request.Form["length"].FirstOrDefault() ?? "10");
            string search = Request.Form["search[value]"].FirstOrDefault() ?? "";

            // permanent constraints for this endpoint
            var baseQuery = context.MasterEkspedisi
                .Where(r => r.Status == status && r.DeletedAt == null);

            // total BEFORE user search, AFTER permanent constraints
            var recordsTotal = baseQuery.Count();

            // apply search
            var filteredQuery = baseQuery;
            if (!string.IsNullOrWhiteSpace(search))
                filteredQuery = filteredQuery.Where(r =>
                    r.NamaEkspedisi.Contains(search));

            var recordsFiltered = filteredQuery.Count();

            // page
            var data = filteredQuery.OrderBy(r => r.IdEkspedisi)
                        .Skip(start)
                        .Take(length)
                        .Select(r => new {
                            r.IdEkspedisi,
                            r.NamaEkspedisi,
                            r.JenisLayanan,
                            r.SlaPengiriman,
                            r.Status
                        })
                        .ToList();

            return Json(new
            {
                draw,
                recordsTotal,
                recordsFiltered,
                data
            });
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(EkspedisiDto ekspedisiDto)
        {
            if (!ModelState.IsValid)
            {
                return View(ekspedisiDto);
            }
            return RedirectToAction("Index");
        }
    }
}
