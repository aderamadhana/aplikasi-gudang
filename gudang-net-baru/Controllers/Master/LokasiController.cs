using gudang_net_baru.Models.Master.Ekspedisi;
using gudang_net_baru.Models.Master.Lokasi;
using gudang_net_baru.Models.Master.Sku;
using gudang_net_baru.Models.Master.Supplier;
using gudang_net_baru.Services;
using Microsoft.AspNetCore.Mvc;

namespace gudang_net_baru.Controllers.Master
{
    public class LokasiController : Controller
    {
        private readonly ApplicationDbContext context;
        public LokasiController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult getLokasi(bool status)
        {

            // DataTables params
            var draw = Request.Form["draw"].FirstOrDefault();
            int start = int.Parse(Request.Form["start"].FirstOrDefault() ?? "0");
            int length = int.Parse(Request.Form["length"].FirstOrDefault() ?? "10");
            string search = Request.Form["search[value]"].FirstOrDefault() ?? "";

            // permanent constraints for this endpoint
            var baseQuery = context.MasterLokasi
                .Where(r => r.Status == status && r.DeletedAt == null);

            // total BEFORE user search, AFTER permanent constraints
            var recordsTotal = baseQuery.Count();

            // apply search
            var filteredQuery = baseQuery;
            if (!string.IsNullOrWhiteSpace(search))
                filteredQuery = filteredQuery.Where(r =>
                    r.Warehouse.Contains(search));

            var recordsFiltered = filteredQuery.Count();

            // page
            var data = filteredQuery.OrderBy(r => r.IdLokasi)
                        .Skip(start)
                        .Take(length)
                        .Select(r => new {
                            r.IdLokasi,
                            r.Warehouse,
                            r.TipeLokasi,
                            r.Kapasitas,
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
        public IActionResult Create(LokasiDto lokasiDto)
        {
            if (!ModelState.IsValid)
            {
                return View(lokasiDto);
            }

            var lokasi = new LokasiEntity()
            {
                Warehouse = lokasiDto.Warehouse,
                TipeLokasi = lokasiDto.TipeLokasi,
                Kapasitas = lokasiDto.Kapasitas,
                Status = true,
                CreatedAt = DateTime.Now,
                CreatedBy = HttpContext.Session.GetString("UserId")
            };

            context.MasterLokasi.Add(lokasi);
            context.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult Edit(string id)
        {
            var lokasi = context.MasterLokasi.Find(id);
            if (lokasi == null)
            {
                return RedirectToAction("Index");
            }

            var lokasi_dto = new LokasiDto()
            {
                Warehouse = lokasi.Warehouse,
                TipeLokasi = lokasi.TipeLokasi,
                Kapasitas = lokasi.Kapasitas,
            };

            ViewData["Id"] = lokasi.IdLokasi;

            return View(lokasi_dto);
        }
    }
}
