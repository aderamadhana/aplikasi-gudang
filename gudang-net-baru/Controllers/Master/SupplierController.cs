using gudang_net_baru.Models.Master.Sku;
using gudang_net_baru.Models.Master.Supplier;
using gudang_net_baru.Models.Master.UnitMeasure;
using gudang_net_baru.Services;
using Microsoft.AspNetCore.Mvc;

namespace gudang_net_baru.Controllers.Master
{
    public class SupplierController : Controller
    {
        private readonly ApplicationDbContext context;
        public SupplierController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult getSupplier(bool status)
        {

            // DataTables params
            var draw = Request.Form["draw"].FirstOrDefault();
            int start = int.Parse(Request.Form["start"].FirstOrDefault() ?? "0");
            int length = int.Parse(Request.Form["length"].FirstOrDefault() ?? "10");
            string search = Request.Form["search[value]"].FirstOrDefault() ?? "";

            // permanent constraints for this endpoint
            var baseQuery = context.MasterSupplier
                .Where(r => r.Status == status && r.DeletedAt == null);

            // total BEFORE user search, AFTER permanent constraints
            var recordsTotal = baseQuery.Count();

            // apply search
            var filteredQuery = baseQuery;
            if (!string.IsNullOrWhiteSpace(search))
                filteredQuery = filteredQuery.Where(r =>
                    r.NamaSupplier.Contains(search));

            var recordsFiltered = filteredQuery.Count();

            // page
            var data = filteredQuery.OrderBy(r => r.IdSupplier)
                        .Skip(start)
                        .Take(length)
                        .Select(r => new {
                            r.IdSupplier,
                            r.KodeSupplier,
                            r.NamaSupplier,
                            r.Alamat,
                            r.TermOfDelivery,
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
        public IActionResult Create(SupplierDto supplierDto)
        {
            if (!ModelState.IsValid)
            {
                return View(supplierDto);
            }

            var supplier = new SupplierEntity()
            {
                KodeSupplier = supplierDto.KodeSupplier,
                NamaSupplier = supplierDto.NamaSupplier,
                Alamat = supplierDto.Alamat,
                TermOfDelivery = supplierDto.TermOfDelivery,
                Status = true,
                CreatedAt = DateTime.Now,
                CreatedBy = HttpContext.Session.GetString("UserId")
            };

            context.MasterSupplier.Add(supplier);
            context.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult Edit(string id)
        {
            var supplier = context.MasterSupplier.Find(id);
            if (supplier == null)
            {
                return RedirectToAction("Index");
            }

            var supplier_dto = new SupplierDto()
            {
                KodeSupplier = supplier.KodeSupplier,
                NamaSupplier = supplier.NamaSupplier,
                Alamat = supplier.Alamat,
                TermOfDelivery = supplier.TermOfDelivery,
            };

            ViewData["Id"] = supplier.IdSupplier;

            return View(supplier_dto);
        }
        [HttpPost]
        public IActionResult Edit(string id, SupplierDto supplierDto)
        {
            var cek = context.MasterSupplier.Find(id);
            if (cek == null)
            {
                return RedirectToAction("Index");
            }
            if (!ModelState.IsValid)
            {
                return View(supplierDto);
            }


            cek.KodeSupplier = supplierDto.KodeSupplier;
            cek.NamaSupplier = supplierDto.NamaSupplier;
            cek.Alamat = supplierDto.Alamat;
            cek.TermOfDelivery = supplierDto.TermOfDelivery;
            cek.Status = true;
            cek.UpdatedAt = DateTime.Now;
            cek.UpdatedBy = HttpContext.Session.GetString("UserId");
            
            context.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult ChangeStatus(string id, bool status)
        {
            var cek = context.MasterSupplier.Find(id);
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
            var cek = context.MasterSupplier.Find(id);
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
