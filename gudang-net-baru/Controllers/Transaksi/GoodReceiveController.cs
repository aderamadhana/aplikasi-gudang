using gudang_net_baru.Models.Transaction.GoodReceive;
using gudang_net_baru.Models.Transaction.PurchaseOrder;
using gudang_net_baru.Services;
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

        public IActionResult GetGoodReceive(string? statusPo, string? supplierId)
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
        public IActionResult Create([FromBody] GoodReceiveDto dto, [FromQuery] bool post = false)
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

            string poNumber = null;
            if (dto.StatusGr == "Posted")
            {
                poNumber = $"#GRN-{today}-{seq.ToString("D4")}";
            }

            var gr = new GoodReceiveEntity()
            {
                GrnNo = dto.GrnNo,
                PoId = dto.PoId,
                TanggalTerima = dto.TanggalTerima,
                UserId = HttpContext.Session.GetString("UserId"),
                StatusGr = dto.StatusGr,
                Status = dto.Status,
                LokasiId = dto.LokasiId,
                LokasiName = dto.LokasiName,
                CreatedAt = DateTime.Now,
                CreatedBy = HttpContext.Session.GetString("UserId"),
            };

            foreach (var d in dto.Details)
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

            context.GoodReceive.Add(gr);
            context.SaveChanges();
            
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
    }
}
