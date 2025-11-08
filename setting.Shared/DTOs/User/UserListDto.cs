namespace setting.Shared.DTOs.User
{
    public class UserListDto
    {
        public int IdUsuario { get; set; }
        public int IdLocal { get; set; }
        public int UsuarioRolId { get; set; }
        public string Rol { get; set; } = null!;
        public string Sucursal { get; set; } = null!;
        public string Usuario { get; set; } = null!;
        public bool Estado { get; set; }

    }
}
