using gudang_net_baru.Models.Transaction.GoodReceive;
using gudang_net_baru.Models.Transaction.PurchaseOrder;
using gudang_net_baru.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace gudang_net_baru.Controllers.Transaksi
{
    public class GoodReceiveController : Controller
    {
        ApplicationDbContext context;
        public GoodReceiveController(ApplicationDbContext context) { 
            this.context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetGoodReceive(string? statusGr)
        {
            // DataTables params
            var draw = Request.Form["draw"].FirstOrDefault();
            int start = int.Parse(Request.Form["start"].FirstOrDefault() ?? "0");
            int length = int.Parse(Request.Form["length"].FirstOrDefault() ?? "10");
            string search = Request.Form["search[value]"].FirstOrDefault() ?? "";

            // permanent constraints for this endpoint
            var baseQuery = context.GoodReceive
                .Where(r => r.DeletedAt == null);

            if (statusGr != null && statusGr != "")
            {
                baseQuery = baseQuery.Where(r => r.StatusGr == statusGr);
            }


            // total BEFORE user search, AFTER permanent constraints
            var recordsTotal = baseQuery.Count();

            // apply search
            var filteredQuery = baseQuery;
            if (!string.IsNullOrWhiteSpace(search))
                filteredQuery = filteredQuery.Where(r =>
                    r.GrnNo.Contains(search));

            var recordsFiltered = filteredQuery.Count();

            // page
            var data = filteredQuery
                .Join(
                    context.PurchaseOrder, 
                    r => r.PoId,           
                    po => po.PurchaseOrderId,    
                    (r, po) => new {           
                        r.GrnId,
                        r.GrnNo,
                        r.TanggalTerima,
                        r.StatusGr,
                        r.LokasiName,
                        po.PoNumber,              
                        po.SupplierName     
                    }
                )
                .OrderBy(r => r.GrnNo)
                .Skip(start)
                .Take(length)
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

        private void ViewBagLoad()
        {
            var purchase_order = context.PurchaseOrder
                .Where(x => x.DeletedAt == null)
                .Where(x => x.StatusPo == "Posted")
                .OrderBy(x => x.PoNumber)
                .Include(x => x.Details)
                .ToList();

            var lokasi_stagging = context.MasterLokasi
                .Where(x => x.DeletedAt == null)
                .ToList();

            ViewBag.PurchaseOrder = purchase_order;
            ViewBag.LokasiStagging = lokasi_stagging;
            ViewBag.UserId = HttpContext.Session.GetString("UserId");
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] GoodReceiveDto dto, [FromQuery] bool post = false)
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

            string today = DateTime.Now.ToString("yyyyMMdd");
            // ambil GRN terakhir hari ini
            var lastGrn = context.GoodReceive
                .Where(x => x.GrnNo.Contains(today))
                .OrderByDescending(x => x.GrnNo)
                .Select(x => x.GrnNo)
                .FirstOrDefault();

            // dapat urutan hari ini
            int seq = 1;
            if (!string.IsNullOrEmpty(lastGrn))
            {
                var lastSeq = lastGrn.Split('-').Last(); // ambil 0001
                int.TryParse(lastSeq, out seq);
                seq++;
            }

            string grNumber = null;
            if (dto.StatusGr == "Posted")
            {
                grNumber = $"#GR-{today}-{seq.ToString("D4")}";
            }

            var gr = new GoodReceiveEntity()
            {
                GrnNo = grNumber,
                PoId = dto.PoId,
                PoNo = dto.PoNo,
                TanggalTerima = dto.TanggalTerima,
                UserId = HttpContext.Session.GetString("UserId"),
                StatusGr = dto.StatusGr,
                Status = dto.Status,
                LokasiId = dto.LokasiId,
                LokasiName = dto.LokasiName,
                CreatedAt = DateTime.Now,
                CreatedBy = HttpContext.Session.GetString("UserId"),
            };

            foreach (var d in dto.Items)
            {
                gr.Details.Add(new GoodReceiveDetailEntity
                {
                    ItemId = d.ItemId,
                    ItemName = d.ItemName,
                    LotNo = d.LotNo,
                    QtyOrder = d.QtyOrder,
                    QtyReceived = d.QtyReceived,
                    Expiry = d.Expiry,
                    StatusQC = d.StatusQC,
                });
            }

            await context.GoodReceive.AddAsync(gr);
            await context.SaveChangesAsync();
            
            if (dto.StatusGr == "Posted")
            {
                var data_po = context.PurchaseOrder
                .Find(dto.PoId);

                data_po.StatusPo = "Closed";
                context.SaveChanges();
            }



            return Ok(new
            {
                message = gr.StatusGr == "Posted" ? "Good Receive berhasil diposting." : "Draft berhasil disimpan.",
                id = gr.GrnId,
                statusGr = gr.StatusGr
            });
        }

        public IActionResult Detail(string id)
        {
            var gr = context.GoodReceive
            .Where(r => r.GrnId == id)
            .Join(context.PurchaseOrder,
                  r => r.PoId,
                  po => po.PurchaseOrderId,
                  (r, po) => new { r, po })
            .GroupJoin(context.Users,
                       rp => rp.r.UserId,
                       u => u.Id,
                       (rp, users) => new { rp.r, rp.po, u = users.FirstOrDefault() })
            .Select(x => new {
                x.r.GrnId,
                x.r.GrnNo,
                x.r.TanggalTerima,
                x.r.StatusGr,
                x.r.LokasiName,
                PoNumber = x.po.PoNumber,
                SupplierName = x.po.SupplierName,
                UserId = x.r.UserId,
                UserName = x.u != null ? x.u.UserName : null,
                FullName = x.u != null ? (x.u.FirstName + " " + x.u.LastName) : null, // jika ada
                Details = x.r.Details.Select(d => new {
                    d.ItemId,
                    d.ItemName,
                    d.QtyOrder,
                    d.QtyReceived,
                    d.StatusQC,
                    d.LotNo,
                    d.Expiry
                }).ToList()
            })
            .FirstOrDefault();

            ViewBag.GoodReceive = gr;
            return View();
        }

        public IActionResult Edit(string id)
        {
            var gr = context.GoodReceive
                .Where(r => r.GrnId == id)
                .Join(context.PurchaseOrder,
                    r => r.PoId,
                    po => po.PurchaseOrderId,
                    (r, po) => new { r, po })
                .GroupJoin(context.Users,
                    rp => rp.r.UserId,
                    u => u.Id,
                    (rp, users) => new { rp.r, rp.po, u = users.FirstOrDefault() })
                .Select(x => new
                {
                    x.r.GrnId,
                    x.r.GrnNo,
                    x.r.TanggalTerima,
                    x.r.StatusGr,
                    x.r.LokasiName,
                    x.r.PoId,
                    x.r.LokasiId,
                    PoNumber = x.po.PoNumber,
                    SupplierName = x.po.SupplierName,
                    UserId = x.r.UserId,
                    UserName = x.u != null ? x.u.UserName : null,
                    FullName = x.u != null ? (x.u.FirstName + " " + x.u.LastName) : null,

                    // join UnitMeasure di level detail
                    Details = x.r.Details
                        .Join(
                            context.MasterSku,                  // tabel/unit measure
                            d => d.ItemId,                        // dari detail (GoodReceiveDetail)
                            sk => sk.IdSku,               // dari UnitMeasure
                            (d, sk) => new
                            {
                                d.ItemId,
                                d.ItemName,
                                d.QtyOrder,
                                d.QtyReceived,
                                d.StatusQC,
                                d.LotNo,
                                d.Expiry,
                                UnitMeasureId = sk.UnitMeasureId,
                                UnitMeasureName = sk.UnitMeasureName     // sesuaikan nama kolom
                            }
                        )
                        .ToList()
                })
                .FirstOrDefault();

            ViewBag.GoodReceive = gr;
            ViewBag.Id = id;

            ViewBagLoad();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] GoodReceiveDto dto, [FromQuery] string grId = "", [FromQuery] bool post = false)
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

            var data_gr = await context.GoodReceive
                .Include(x => x.Details)
                .FirstOrDefaultAsync(x => x.GrnId == grId);

            if (data_gr == null)
            {
                return BadRequest(new
                {
                    error = true,
                    message = "Data good receive tidak ditemukan!"
                });
            }

            string today = DateTime.Now.ToString("yyyyMMdd");

            // ambil GR terakhir hari ini
            var lastGr = context.GoodReceive
                .Where(x => x.GrnNo.Contains(today))
                .OrderByDescending(x => x.GrnNo)
                .Select(x => x.GrnNo)
                .FirstOrDefault();

            // dapat urutan hari ini
            int seq = 1;
            if (!string.IsNullOrEmpty(lastGr))
            {
                var lastSeq = lastGr.Split('-').Last(); // ambil 0001
                int.TryParse(lastSeq, out seq);
                seq++;
            }

            string grNumber = null;
            if (dto.StatusGr == "Posted")
            {
                grNumber = $"#GR-{today}-{seq.ToString("D4")}";
            }

            data_gr.GrnNo = grNumber;
            data_gr.PoId = dto.PoId;
            data_gr.PoNo = dto.PoNo;
            data_gr.TanggalTerima = dto.TanggalTerima;
            data_gr.UserId = HttpContext.Session.GetString("UserId");
            data_gr.StatusGr = dto.StatusGr;
            data_gr.Status = dto.Status;
            data_gr.LokasiId = dto.LokasiId;
            data_gr.LokasiName = dto.LokasiName;
            data_gr.UpdatedAt = DateTime.Now;
            data_gr.UpdatedBy = HttpContext.Session.GetString("UserId");


            data_gr.Details.Clear();
            foreach (var d in dto.Items)
            {
                data_gr.Details.Add(new GoodReceiveDetailEntity
                {
                    ItemId = d.ItemId,
                    ItemName = d.ItemName,
                    LotNo = d.LotNo,
                    QtyOrder = d.QtyOrder,
                    QtyReceived = d.QtyReceived,
                    Expiry = d.Expiry,
                    StatusQC = d.StatusQC,
                });
            }

            await context.SaveChangesAsync();

            return Ok(new
            {
                message = data_gr.StatusGr == "Posted" ? "Good Receive berhasil diposting." : "Draft berhasil disimpan.",
                id = data_gr.GrnId,
                statusPo = data_gr.StatusGr
            });
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            var gr = context.GoodReceive.Find(id);
            if (gr is null)
            {
                return Json(new
                {
                    success = false,
                    message = "Gagal hapus data"
                });
            }

            gr.Status = false;
            gr.DeletedAt = DateTime.Now;
            gr.DeletedBy = HttpContext.Session.GetString("UserId");

            context.SaveChanges();

            return Json(new
            {
                success = true,
                message = "Berhasil hapus data"
            });

        }

    }
}
