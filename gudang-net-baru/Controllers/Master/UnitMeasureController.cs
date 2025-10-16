using gudang_net_baru.Models.Master.Supplier;
using gudang_net_baru.Models.Master.UnitMeasure;
using gudang_net_baru.Services;
using Microsoft.AspNetCore.Mvc;

namespace gudang_net_baru.Controllers.Master
{
    public class UnitMeasureController : Controller
    {
        private readonly ApplicationDbContext context;
        public UnitMeasureController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult getUnitMeasure(bool status)
        {

            // DataTables params
            var draw = Request.Form["draw"].FirstOrDefault();
            int start = int.Parse(Request.Form["start"].FirstOrDefault() ?? "0");
            int length = int.Parse(Request.Form["length"].FirstOrDefault() ?? "10");
            string search = Request.Form["search[value]"].FirstOrDefault() ?? "";

            // permanent constraints for this endpoint
            var baseQuery = context.MasterUnitMeasure
                .Where(r => r.Status == status && r.DeletedAt == null);

            // total BEFORE user search, AFTER permanent constraints
            var recordsTotal = baseQuery.Count();

            // apply search
            var filteredQuery = baseQuery;
            //if (!string.IsNullOrWhiteSpace(search))
            //    filteredQuery = filteredQuery.Where(r =>
            //        r.ToString(Conversion).Contains(search));

            var recordsFiltered = filteredQuery.Count();

            // page
            var data = filteredQuery.OrderBy(r => r.IdUnitMeasure)
                        .Skip(start)
                        .Take(length)
                        .Select(r => new {
                            r.IdUnitMeasure,
                            r.Box,
                            r.Carton,
                            r.Conversion,
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
        public IActionResult Create(UnitMeasureDto unitMeasureDto)
        {
            if (!ModelState.IsValid)
            {
                return View(unitMeasureDto);
            }
            return RedirectToAction("Index");
        }
    }
}
