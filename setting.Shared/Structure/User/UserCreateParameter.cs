namespace setting.Shared.Structure.User
{
    public class UserCreateParameter
    {
        public int UsuarioRolId { get; set; }
        public int IdLocal { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string RolUsuario { get; set; } = string.Empty;

    }
}
