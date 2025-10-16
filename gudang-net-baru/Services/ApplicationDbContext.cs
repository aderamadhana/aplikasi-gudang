﻿using gudang_net_baru.Models;
using gudang_net_baru.Models.Konfigurasi.Menu;
using gudang_net_baru.Models.Master.Customer;
using gudang_net_baru.Models.Master.Ekspedisi;
using gudang_net_baru.Models.Master.Lokasi;
using gudang_net_baru.Models.Master.ReasonCode;
using gudang_net_baru.Models.Master.Sku;
using gudang_net_baru.Models.Master.Supplier;
using gudang_net_baru.Models.Master.UnitMeasure;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace gudang_net_baru.Services
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<MenuEntity> Menu { get; set; }
        public DbSet<CustomerEntity> MasterCustomer { get; set; }
        public DbSet<EkspedisiEntity> MasterEkspedisi { get; set; }
        public DbSet<LokasiEntity> MasterLokasi { get; set; }
        public DbSet<ReasonCodeEntity> MasterReasonCode { get; set; }
        public DbSet<SkuEntity> MasterSku { get; set; }
        public DbSet<SupplierEntity> MasterSupplier { get; set; }
        public DbSet<UnitMeasureEntity> MasterUnitMeasure { get; set; }
    }
}
