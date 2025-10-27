using gudang_net_baru.Models.Master.ReasonCode;
using gudang_net_baru.Models.Master.Sku;
using gudang_net_baru.Models.Master.Supplier;
using gudang_net_baru.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace gudang_net_baru.Controllers.Master
{
    [Authorize]
    public class SkuController : Controller
    {
        private readonly ApplicationDbContext context;
        public SkuController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult getSku(bool status)
        {

            // DataTables params
            var draw = Request.Form["draw"].FirstOrDefault();
            int start = int.Parse(Request.Form["start"].FirstOrDefault() ?? "0");
            int length = int.Parse(Request.Form["length"].FirstOrDefault() ?? "10");
            string search = Request.Form["search[value]"].FirstOrDefault() ?? "";

            // permanent constraints for this endpoint
            var baseQuery = context.MasterSku
                .Where(r => r.Status == status && r.DeletedAt == null);

            // total BEFORE user search, AFTER permanent constraints
            var recordsTotal = baseQuery.Count();

            // apply search
            var filteredQuery = baseQuery;
            if (!string.IsNullOrWhiteSpace(search))
                filteredQuery = filteredQuery.Where(r =>
                    r.NamaBarang.Contains(search));

            var recordsFiltered = filteredQuery.Count();

            // page
            var data = filteredQuery.OrderBy(r => r.IdSku)
                        .Skip(start)
                        .Take(length)
                        .Select(r => new {
                            r.IdSku,
                            r.NamaBarang,
                            r.KodeSku,
                            r.JenisBarang,
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
        public async Task<IActionResult> Create()
        {
            var list_unit_measure = await context.MasterUnitMeasure
                .Where(m => m.Status == true)
                .Select(m => new {
                    m.IdUnitMeasure,
                    m.UnitMeasureName,
                    m.Status
                })
                .ToListAsync();

            ViewBag.UnitMeasure = list_unit_measure;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(SkuDto skuDto)
        {
            if (!ModelState.IsValid)
            {
                var list_unit_measure = await context.MasterUnitMeasure
                .Where(m => m.Status == true)
                .Select(m => new {
                    m.IdUnitMeasure,
                    m.UnitMeasureName,
                    m.Status
                })
                .ToListAsync();

                ViewBag.UnitMeasure = list_unit_measure;
                return View(skuDto);
            }

            var unit_measure = context.MasterUnitMeasure.Find(skuDto.UnitMeasureId);

            var sku = new SkuEntity()
            {
                KodeSku = skuDto.KodeSku,
                NamaBarang = skuDto.NamaBarang,
                Deskripsi = skuDto.Deskripsi,
                UnitMeasureId = skuDto.UnitMeasureId,
                UnitMeasureName = unit_measure?.UnitMeasureName,
                Dimensi = skuDto.Dimensi,
                JenisBarang = skuDto.JenisBarang,
                LotTracking = skuDto.LotTracking,
                Status = true,
                CreatedAt = DateTime.Now,
                CreatedBy = HttpContext.Session.GetString("UserId")
            };

            context.MasterSku.Add(sku);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(string id)
        {
            var sku = context.MasterSku.Find(id);
            if (sku == null)
            {
                return RedirectToAction("Index");
            }


            var sku_dto = new SkuDto()
            {
                KodeSku = sku.KodeSku,
                NamaBarang = sku.NamaBarang,
                Deskripsi = sku.Deskripsi,
                UnitMeasureId = sku.UnitMeasureId,
                Dimensi = sku.Dimensi,
                JenisBarang = sku.JenisBarang,
                LotTracking = sku.LotTracking,
            };

            ViewData["Id"] = sku.IdSku;

            var list_unit_measure = await context.MasterUnitMeasure
                .Where(m => m.Status == true)
                .Select(m => new {
                    m.IdUnitMeasure,
                    m.UnitMeasureName,
                    m.Status
                })
                .ToListAsync();

            ViewBag.UnitMeasure = list_unit_measure;
            return View(sku_dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, SkuDto skuDto)
        {
            var cek = context.MasterSku.Find(id);
            if (cek == null)
            {
                return RedirectToAction("Index");
            }
            if (!ModelState.IsValid)
            {
                var list_unit_measure = await context.MasterUnitMeasure
                .Where(m => m.Status == true)
                .Select(m => new {
                    m.IdUnitMeasure,
                    m.UnitMeasureName,
                    m.Status
                })
                .ToListAsync();

                ViewBag.UnitMeasure = list_unit_measure;
                return View(skuDto);
            }

            var unit_measure = context.MasterUnitMeasure.Find(skuDto.UnitMeasureId);


            cek.KodeSku = skuDto.KodeSku;
            cek.NamaBarang = skuDto.NamaBarang;
            cek.Deskripsi = skuDto.Deskripsi;
            cek.UnitMeasureId = skuDto.UnitMeasureId;
            cek.UnitMeasureName = unit_measure?.UnitMeasureName;
            cek.Dimensi = skuDto.Dimensi;
            cek.JenisBarang = skuDto.JenisBarang;
            cek.LotTracking = skuDto.LotTracking;
            cek.Status = true;
            cek.CreatedAt = DateTime.Now;
            cek.CreatedBy = HttpContext.Session.GetString("UserId");
            
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ChangeStatus(string id, bool status)
        {
            var cek = context.MasterSku.Find(id);
            if (cek == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Gagal ubah status!"
                });
            }

            cek.Status = status;
            cek.CreatedAt = DateTime.Now;
            cek.CreatedBy = HttpContext.Session.GetString("UserId");

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
            var cek = context.MasterSku.Find(id);
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
