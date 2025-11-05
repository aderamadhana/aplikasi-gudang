using gudang_net_baru.Models;
using gudang_net_baru.Models.Transaction.PurchaseOrder;
using gudang_net_baru.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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
            ViewBagLoad();
            return View();
        }

        public IActionResult GetPurchaseOrder(string? statusPo, string? supplierId)
        {
            // DataTables params
            var draw = Request.Form["draw"].FirstOrDefault();
            int start = int.Parse(Request.Form["start"].FirstOrDefault() ?? "0");
            int length = int.Parse(Request.Form["length"].FirstOrDefault() ?? "10");
            string search = Request.Form["search[value]"].FirstOrDefault() ?? "";

            // permanent constraints for this endpoint
            var baseQuery = context.PurchaseOrder
                .Where(r => r.DeletedAt == null);

            if (statusPo != null && statusPo != "")
            {
                baseQuery = baseQuery.Where(r => r.StatusPo == statusPo);
            }

            if (supplierId != null && supplierId != "")
            {
                baseQuery = baseQuery.Where(r => r.SupplierId == supplierId);
            }


            // total BEFORE user search, AFTER permanent constraints
            var recordsTotal = baseQuery.Count();

            // apply search
            var filteredQuery = baseQuery;
            if (!string.IsNullOrWhiteSpace(search))
                filteredQuery = filteredQuery.Where(r =>
                    r.PoNumber.Contains(search));

            var recordsFiltered = filteredQuery.Count();

            // page
            var data = filteredQuery.OrderBy(r => r.PurchaseOrderId)
                        .Skip(start)
                        .Take(length)
                        .Select(r => new {
                            r.PurchaseOrderId,
                            r.PoNumber,
                            r.SupplierName,
                            r.TanggalPo,
                            r.StatusPo,
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
            ViewBagLoad();

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PurchaseOrderDto dto, [FromQuery] bool post = false)
        {
            if(dto is null)
            {
                ViewBagLoad();
                return BadRequest(new { 
                    error = true,
                    message = "Harap isi semua data!" 
                });
            }

            if(dto.TotalQty == 0)
            {
                ViewBagLoad();
                return BadRequest(new
                {
                    error = true,
                    message = "Items Order tidak boleh kosong!"
                });
            }

            string today = DateTime.Now.ToString("yyyyMMdd");

            // ambil PO terakhir hari ini
            var lastPo = context.PurchaseOrder
                .Where(x => x.PoNumber.Contains(today))
                .OrderByDescending(x => x.PoNumber)
                .Select(x => x.PoNumber)
                .FirstOrDefault();

            // dapat urutan hari ini
            int seq = 1;
            if (!string.IsNullOrEmpty(lastPo))
            {
                var lastSeq = lastPo.Split('-').Last(); // ambil 0001
                int.TryParse(lastSeq, out seq);
                seq++;
            }

            string poNumber = null;
            if (dto.StatusPo == "Posted")
            { 
                poNumber = $"#PO-{today}-{seq.ToString("D4")}";
            }

            var po = new PurchaseOrderEntity()
            {
                SupplierId = dto.SupplierId,
                SupplierName = dto.SupplierName,
                PoNumber = poNumber,
                TanggalPo = dto.TanggalPo.ToDateTime(TimeOnly.MinValue),
                StatusPo = dto.StatusPo,
                TotalQty = dto.TotalQty,
                Keterangan = dto.Keterangan,
                Status = dto.Status,
                CreatedAt = DateTime.Now,
                CreatedBy = HttpContext.Session.GetString("UserId"),
            };

            foreach(var d in dto.Items)
            {
                po.Details.Add(new PurchaseOrderDetailEntity
                {
                    ItemId = d.ItemId,
                    ItemName = d.ItemName,
                    QtyOrder = d.QtyOrder,
                    QtyReceived = d.QtyReceived,
                    UnitMeasureName = d.UnitMeasureName,
                    Keterangan = d.KeteranganNotes
                });
            }

            await context.PurchaseOrder.AddAsync(po);
            await context.SaveChangesAsync();

            return Ok(new
            {
                message = po.StatusPo == "Posted" ? "Purchase Order berhasil diposting." : "Draft berhasil disimpan.",
                id = po.PurchaseOrderId,
                statusPo = po.StatusPo,
                totalQty = po.TotalQty
            });
        }
        public IActionResult Detail(string id)
        {
            ViewBagLoad();

            var purchase_order = context.PurchaseOrder
                .Include(x => x.Details)
                .FirstOrDefault(x => x.PurchaseOrderId == id);

            if (purchase_order == null)
            {
                return RedirectToAction("Index");
            }

            var dto = new PurchaseOrderDto
            {
                PoNumber = purchase_order.PoNumber,
                SupplierId = purchase_order.SupplierId,
                SupplierName = purchase_order.SupplierName,
                TanggalPo = DateOnly.FromDateTime((DateTime)purchase_order.TanggalPo),
                TotalQty = purchase_order.TotalQty,
                Keterangan = purchase_order.Keterangan,
                Status = purchase_order.Status,
                StatusPo = purchase_order.StatusPo,
                CreatedAt = purchase_order.CreatedAt,
                CreatedBy = purchase_order.CreatedBy,
                Items = purchase_order.Details.Select(i => new PurchaseOrderDetailDto
                {
                    ItemId = i.ItemId,
                    ItemName = i.ItemName,
                    QtyOrder = i.QtyOrder,
                    QtyReceived = i.QtyReceived,
                    UnitMeasureName = i.UnitMeasureName,
                    KeteranganNotes = i.Keterangan,
                }).ToList()

            };

            ViewBag.Id = id;

            return View(dto);
        }
        public IActionResult Edit(string id)
        {
            ViewBagLoad();

            var purchase_order = context.PurchaseOrder
                .Include(x => x.Details)
                .FirstOrDefault(x => x.PurchaseOrderId == id);

            if (purchase_order == null)
            {
                return RedirectToAction("Index");
            }

            var dto = new PurchaseOrderDto
            {
                PoNumber = purchase_order.PoNumber,
                SupplierId = purchase_order.SupplierId,
                SupplierName = purchase_order.SupplierName,
                TanggalPo = DateOnly.FromDateTime((DateTime)purchase_order.TanggalPo),
                TotalQty = purchase_order.TotalQty,
                Keterangan = purchase_order.Keterangan,
                Status = purchase_order.Status,
                CreatedAt = purchase_order.CreatedAt,
                CreatedBy = purchase_order.CreatedBy,
                Items = purchase_order.Details.Select(i => new PurchaseOrderDetailDto
                {
                    ItemId = i.ItemId,
                    ItemName = i.ItemName,
                    QtyOrder = i.QtyOrder,
                    QtyReceived = i.QtyReceived,
                    UnitMeasureName = i.UnitMeasureName,
                    KeteranganNotes = i.Keterangan,
                }).ToList()

            };

            ViewBag.Id = id;

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] PurchaseOrderDto dto, [FromQuery] string poId = "", [FromQuery] bool post = false)
        {
            if (dto is null)
            {
                ViewBagLoad();
                return BadRequest(new
                {
                    error = true,
                    message = "Harap isi semua data!"
                });
            }

            if (dto.TotalQty == 0)
            {
                ViewBagLoad();
                return BadRequest(new
                {
                    error = true,
                    message = "Items Order tidak boleh kosong!"
                });
            }

            var data_po = await context.PurchaseOrder
                .Include(x => x.Details)
                .FirstOrDefaultAsync(x => x.PurchaseOrderId == poId);

            if(data_po == null)
            {
                return BadRequest(new
                {
                    error = true,
                    message = "Data purchase order tidak ditemukan!"
                });
            }

            string today = DateTime.Now.ToString("yyyyMMdd");

            // ambil PO terakhir hari ini
            var lastPo = context.PurchaseOrder
                .Where(x => x.PoNumber.Contains(today))
                .OrderByDescending(x => x.PoNumber)
                .Select(x => x.PoNumber)
                .FirstOrDefault();

            // dapat urutan hari ini
            int seq = 1;
            if (!string.IsNullOrEmpty(lastPo))
            {
                var lastSeq = lastPo.Split('-').Last(); // ambil 0001
                int.TryParse(lastSeq, out seq);
                seq++;
            }

            string poNumber = null;
            if (dto.StatusPo == "Posted")
            {
                poNumber = $"#PO-{today}-{seq.ToString("D4")}";
            }

            data_po.SupplierId = dto.SupplierId;
            data_po.SupplierName = dto.SupplierName;
            data_po.PoNumber = poNumber;
            data_po.TanggalPo = dto.TanggalPo.ToDateTime(TimeOnly.MinValue);
            data_po.StatusPo = dto.StatusPo;
            data_po.TotalQty = dto.TotalQty;
            data_po.UpdatedAt = DateTime.Now;
            data_po.UpdatedBy = HttpContext.Session.GetString("UserId");


            data_po.Details.Clear();
            foreach (var d in dto.Items)
            {
                data_po.Details.Add(new PurchaseOrderDetailEntity
                {
                    ItemId = d.ItemId,
                    ItemName = d.ItemName,
                    QtyOrder = d.QtyOrder,
                    QtyReceived = d.QtyReceived,
                    UnitMeasureName = d.UnitMeasureName,
                    Keterangan = d.KeteranganNotes
                });
            }

            await context.SaveChangesAsync();

            return Ok(new
            {
                message = data_po.StatusPo == "Posted" ? "Purchase Order berhasil diposting." : "Draft berhasil disimpan.",
                id = data_po.PurchaseOrderId,
                statusPo = data_po.StatusPo,
                totalQty = data_po.TotalQty
            });
        }

        private void ViewBagLoad()
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
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            var po = context.PurchaseOrder.Find(id);
            if(po is null)
            {
                return Json(new
                {
                    success = false,
                    message = "Gagal hapus data"
                });
            }

            po.Status = false;
            po.DeletedAt = DateTime.Now;
            po.DeletedBy = HttpContext.Session.GetString("UserId");

            context.SaveChanges();

            return Json(new
            {
                success = true,
                message = "Berhasil hapus data"
            });

        }
    }
}
