using gudang_net_baru.Models.Master.Customer;
using gudang_net_baru.Models.Master.Ekspedisi;
using gudang_net_baru.Models.Master.Sku;
using gudang_net_baru.Models.Master.Supplier;
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

            var ekspedisi = new EkspedisiEntity()
            {
                NamaEkspedisi = ekspedisiDto.NamaEkspedisi,
                SlaPengiriman = ekspedisiDto.SlaPengiriman,
                JenisLayanan = ekspedisiDto.JenisLayanan,
                Status = true,
                CreatedAt = DateTime.Now,
                CreatedBy = HttpContext.Session.GetString("UserId"),
            };

            context.MasterEkspedisi.Add(ekspedisi);
            context.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult Edit(string id)
        {
            var ekspedisi = context.MasterEkspedisi.Find(id);
            if (ekspedisi == null)
            {
                return RedirectToAction("Index");
            }

            var ekspedisi_dto = new EkspedisiDto()
            {
                NamaEkspedisi = ekspedisi.NamaEkspedisi,
                SlaPengiriman = ekspedisi.SlaPengiriman,
                JenisLayanan = ekspedisi.JenisLayanan,
            };

            ViewData["Id"] = ekspedisi.IdEkspedisi;

            return View(ekspedisi_dto);
        }

        [HttpPost]
        public IActionResult Edit(string id, EkspedisiDto ekspedisiDto)
        {
            var cek = context.MasterEkspedisi.Find(id);
            if (cek == null)
            {
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                return View(ekspedisiDto);
            }


            cek.NamaEkspedisi = ekspedisiDto.NamaEkspedisi;
            cek.SlaPengiriman = ekspedisiDto.SlaPengiriman;
            cek.JenisLayanan = ekspedisiDto.JenisLayanan;
            cek.Status = true;
            cek.UpdatedAt = DateTime.Now;
            cek.UpdatedBy = HttpContext.Session.GetString("UserId");
            
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ChangeStatus(string id, bool status)
        {
            var cek = context.MasterEkspedisi.Find(id);
            if (cek == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Gagal ubah status!"
                });
            }
            
            cek.Status = status;
            cek.UpdatedAt = DateTime.Now;
            cek.UpdatedBy = HttpContext.Session.GetString("UserId");

            context.SaveChanges();

            return Json(new
            {
                success = true,
                message = "Berhasil ubah status!"
            });
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            var cek = context.MasterEkspedisi.Find(id);
            if (cek == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Gagal hapus data!"
                });
            }

            cek.DeletedAt = DateTime.Now;
            cek.DeletedBy = HttpContext.Session.GetString("UserId");

            context.SaveChanges();

            return Json(new
            {
                success = true,
                message = "Berhasil hapus data!"
            });
        }

    }
}
