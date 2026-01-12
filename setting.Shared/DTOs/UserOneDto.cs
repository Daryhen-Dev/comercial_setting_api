namespace setting.Shared.DTOs
{
    public class UserOneDto
    {
        public int IdLocal { get; set; }
        public int UsuarioRolId { get; set; }
        public int IdUsuario { get; set; }
        public string Sucursal { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string RolName { get; set; } = string.Empty;
    }
}
