using gudang_net_baru.Models.Master.Customer;
using gudang_net_baru.Models.Master.Sku;
using gudang_net_baru.Models.Master.Supplier;
using gudang_net_baru.Services;
using Microsoft.AspNetCore.Mvc;

namespace gudang_net_baru.Controllers.Master
{
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext context;
        public CustomerController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult getCustomer(bool status)
        {

            // DataTables params
            var draw = Request.Form["draw"].FirstOrDefault();
            int start = int.Parse(Request.Form["start"].FirstOrDefault() ?? "0");
            int length = int.Parse(Request.Form["length"].FirstOrDefault() ?? "10");
            string search = Request.Form["search[value]"].FirstOrDefault() ?? "";

            // permanent constraints for this endpoint
            var baseQuery = context.MasterCustomer
                .Where(r => r.Status == status && r.DeletedAt == null);

            // total BEFORE user search, AFTER permanent constraints
            var recordsTotal = baseQuery.Count();

            // apply search
            var filteredQuery = baseQuery;
            if (!string.IsNullOrWhiteSpace(search))
                filteredQuery = filteredQuery.Where(r =>
                    r.NamaCustomer.Contains(search));

            var recordsFiltered = filteredQuery.Count();

            // page
            var data = filteredQuery.OrderBy(r => r.IdCustomer)
                        .Skip(start)
                        .Take(length)
                        .Select(r => new {
                            r.IdCustomer,
                            r.NamaCustomer,
                            r.Alamat,
                            r.JenisCustomer,
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
        public IActionResult Create(CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
            {
                return View(customerDto);
            }

            var customer = new CustomerEntity()
            {
                NamaCustomer = customerDto.NamaCustomer,
                Alamat = customerDto.Alamat,
                JenisCustomer = customerDto.JenisCustomer,
                Status = true,
                CreatedAt = DateTime.Now,
                CreatedBy = HttpContext.Session.GetString("UserId"),
            };

            context.MasterCustomer.Add(customer);
            context.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult Edit(string id)
        {
            var customer = context.MasterCustomer.Find(id);
            if (customer == null)
            {
                return RedirectToAction("Index");
            }

            var customer_dto = new CustomerDto()
            {
                NamaCustomer = customer.NamaCustomer,
                Alamat = customer.Alamat,
                JenisCustomer = customer.JenisCustomer,
            };

            ViewData["Id"] = customer.IdCustomer;

            return View(customer_dto);
        }
    }
}
