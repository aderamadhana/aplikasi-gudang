using gudang_net_baru.Models.Master.Lokasi;
using gudang_net_baru.Models.Master.ReasonCode;
using gudang_net_baru.Models.Master.Sku;
using gudang_net_baru.Models.Master.Supplier;
using gudang_net_baru.Services;
using Microsoft.AspNetCore.Mvc;

namespace gudang_net_baru.Controllers.Master
{
    public class ReasonCodeController : Controller
    {
        private readonly ApplicationDbContext context;
        public ReasonCodeController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult getReasonCode(bool status)
        {

            // DataTables params
            var draw = Request.Form["draw"].FirstOrDefault();
            int start = int.Parse(Request.Form["start"].FirstOrDefault() ?? "0");
            int length = int.Parse(Request.Form["length"].FirstOrDefault() ?? "10");
            string search = Request.Form["search[value]"].FirstOrDefault() ?? "";

            // permanent constraints for this endpoint
            var baseQuery = context.MasterReasonCode
                .Where(r => r.Status == status && r.DeletedAt == null);

            // total BEFORE user search, AFTER permanent constraints
            var recordsTotal = baseQuery.Count();

            // apply search
            var filteredQuery = baseQuery;
            if (!string.IsNullOrWhiteSpace(search))
                filteredQuery = filteredQuery.Where(r =>
                    r.Kategori.Contains(search));

            var recordsFiltered = filteredQuery.Count();

            // page
            var data = filteredQuery.OrderBy(r => r.IdReasonCode)
                        .Skip(start)
                        .Take(length)
                        .Select(r => new {
                            r.IdReasonCode,
                            r.Kategori,
                            r.ReasonCode,
                            r.Deskripsi,
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
        public IActionResult Create(ReasonCodeDto reasonCodeDto)
        {
            if (!ModelState.IsValid)
            {
                return View(reasonCodeDto);
            }

            var reason = new ReasonCodeEntity()
            {
                Kategori = reasonCodeDto.Kategori,
                ReasonCode = reasonCodeDto.ReasonCode,
                Deskripsi = reasonCodeDto.Deskripsi,
                Status = true,
                CreatedAt = DateTime.Now,
                CreatedBy = HttpContext.Session.GetString("UserId"),
            };

            context.MasterReasonCode.Add(reason);
            context.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult Edit(string id)
        {
            var reason_code = context.MasterReasonCode.Find(id);
            if (reason_code == null)
            {
                return RedirectToAction("Index");
            }

            var reason_code_dto = new ReasonCodeDto()
            {
                Kategori = reason_code.Kategori,
                ReasonCode = reason_code.ReasonCode,
                Deskripsi = reason_code.Deskripsi,
            };

            ViewData["Id"] = reason_code.IdReasonCode;

            return View(reason_code_dto);
        }

        [HttpPost]
        public IActionResult Edit(string id, ReasonCodeDto reasonCodeDto)
        {
            var cek = context.MasterReasonCode.Find(id);
            if (cek == null)
            {
                return RedirectToAction("Index");
            }
            if (!ModelState.IsValid)
            {
                return View(reasonCodeDto);
            }


            cek.Kategori = reasonCodeDto.Kategori;
            cek.ReasonCode = reasonCodeDto.ReasonCode;
            cek.Deskripsi = reasonCodeDto.Deskripsi;
            cek.Status = true;
            cek.UpdatedAt = DateTime.Now;
            cek.UpdatedBy = HttpContext.Session.GetString("UserId");
            
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ChangeStatus(string id, bool status)
        {
            var cek = context.MasterReasonCode.Find(id);
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
            var cek = context.MasterReasonCode.Find(id);
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
