namespace setting.Shared.DTOs
{
    public class IpLoginDto
    {
        public int AjusteIpSucursalId { get; set; }
        public int IdLocal { get; set; }
        public string Sucursal { get; set; } = string.Empty;
        public string Detalle { get; set; } = string.Empty;
        public string Ip { get; set; } = string.Empty;
        public DateTime DateCreate { get; set; }
        public DateTime DateUpdate { get; set; }

    }
}
