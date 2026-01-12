namespace setting.Shared.Structure.User
{
    public class UserUpdatedParemeter
    {
         public int IdUsuario { get; set; }
         public int UsuarioRolId { get; set; }
         public string Usuario { get; set; } = string.Empty;
         public string Password { get; set; } = string.Empty;
    }
}
