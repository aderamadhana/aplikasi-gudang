namespace gudang_net_baru.Models.Konfigurasi.Menu
{
    public class SaveMenuOrderRequest
    {
        public string RoleId { get; set; }
        public List<MenuOrderFlat> Items { get; set; }     }

    public class MenuOrderFlat
    {
        public string Id { get; set; }
        public string? ParentId { get; set; }
        public int Order { get; set; }
    }
}
