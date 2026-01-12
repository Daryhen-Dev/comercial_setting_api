using Dapper;
using setting.Shared.DTOs;
using setting.Shared.Structure;
using setting.Shared.Structure.User;
using System.Data;
using System.Security.Claims;

namespace setting.Dapper.User
{
    public class UserCad : IUserCad
    {
        private readonly IDbConnection _dbConnection;

        public UserCad(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<FetchResponse> CreateUserAsync(UserCreateParameter newUser)
        {
            var sql = @"IF NOT EXISTS (
            SELECT 1 FROM Usuario 
            WHERE IDLocal = @IdLocal AND NombreUsuario = @Usuario
            )
            BEGIN
            INSERT INTO Usuario
                ([UsuarioRolId], [IDLocal], [NombreUsuario], [Clave], [RolUsuario], [Estado])
            VALUES
                (@UsuarioRolId, @IdLocal, @Usuario, @Password, @RolUsuario, 1);          
            SELECT  1 as success, 'Usuario creado exitosamente.' as message, NULL as data;
            END
            ELSE
            BEGIN
            SELECT  0 as success, 'Usuario ya existe para este local' as message, NULL as data;
            END";
            var result = await _dbConnection.QueryFirstAsync<dynamic>(sql,
                new
                {
                    newUser.UsuarioRolId,
                    newUser.IdLocal,
                    newUser.Usuario,
                    newUser.Password,
                    newUser.RolUsuario
                });
            return new FetchResponse
            {
                Success = result.success == 1 ? true : false,
                Message = result.message,
                Data = result.data,
            };
        }

        public async Task<UserListDto> GetOneUserAsync(int idLocal, string user, string password)
        {
            var sql = @"SELECT U.IDUsuario IdUsuario, ISNULL(U.UsuarioRolId, -1) UsuarioRolId, L.IDLocal IdLocal, ISNULL(UR.Descripcion, 'ERROR' ) Rol, L.[Local] Sucursal, U.NombreUsuario Usuario, U.Estado
                        FROM Usuario U
                        INNER JOIN Usuario_Rol UR ON U.UsuarioRolId = UR.UsuarioRolId
                        INNER JOIN Local L ON U.IDLocal = L.IDLocal
                        WHERE U.IDLocal = @IdLocal AND U.NombreUsuario = @Usuario AND U.Clave = @Password";
            var result = await _dbConnection.QueryFirstOrDefaultAsync<UserListDto>(sql,
                new
                {
                    IdLocal = idLocal,
                    Usuario = user,
                    Password = password
                });
            if (result == null)
            {
                return new UserListDto();
            }
            return result;
        }

        public async Task<int> GetUserExistAsync(int idUser)
        {
            var sql = @"SELECT ISNULL(U.UsuarioRolId, 2) UsuarioRolId
                        FROM Usuario U
                        WHERE U.IDUsuario = @IdUsuario ";
            var result = await _dbConnection.QueryFirstOrDefaultAsync<int?>(sql,
                new
                {
                    IdUsuario = idUser,
                });
            if (result.HasValue)
            {
                return result.Value;
            }
            else
            {
                return 0; // Valor por defecto si no se encuentra el usuario
            }
        }

        public async Task<IEnumerable<UserListDto>> GetUserListAsync()
        {
            var sqlAll = @"SELECT U.IDUsuario IdUsuario, ISNULL(U.UsuarioRolId, -1) UsuarioRolId, L.IDLocal IdLocal, ISNULL(UR.Descripcion, 'ERROR' ) Rol, L.[Local] Sucursal, U.NombreUsuario Usuario, U.Estado
                FROM Usuario U
                LEFT JOIN Usuario_Rol UR ON U.UsuarioRolId = UR.UsuarioRolId
                INNER JOIN Local L ON U.IDLocal = L.IDLocal
                ORDER BY U.NombreUsuario ASC";
            IEnumerable<UserListDto> result = await _dbConnection.QueryAsync<UserListDto>(sqlAll);
            return result;
        }

        public async Task<FetchResponse> GetUserOneAsync(UserLoginParameter userLoginParameter)
        {
            var sql = @"
        SELECT 
            U.IDUsuario IdUsuario,
            ISNULL(U.UsuarioRolId, -1) UsuarioRolId,
            L.IDLocal IdLocal,
            ISNULL(UR.Descripcion, 'ERROR') RolName,
            L.[Local] Sucursal,
            U.NombreUsuario Name
        FROM Usuario U
        LEFT JOIN Usuario_Rol UR ON U.UsuarioRolId = UR.UsuarioRolId
        INNER JOIN Local L ON U.IDLocal = L.IDLocal
        WHERE U.IDLocal = @IdLocal AND U.NombreUsuario = @Usuario AND U.Clave = @Password
    ";

            var user = await _dbConnection.QueryFirstOrDefaultAsync<UserOneDto>(sql,
                new
                {
                    userLoginParameter.IdLocal,
                    userLoginParameter.Usuario,
                    userLoginParameter.Password
                });

            if (user == null)
            {
                return new FetchResponse
                {
                    Success = false,
                    Message = "Usuario o contraseña incorrecta",
                    Data = ""
                };
            }

            return new FetchResponse
            {
                Success = true,
                Message = "Usuario logueado.",
                Data = user
            };
        }

        public async Task UpdatedUserActiveAsync(UserUpdatedActiveParameter userActive)
        {
            var sql = @"UPDATE Usuario
                        SET Estado = @Estado
                        WHERE IDUsuario = @IdUsuario";
            await _dbConnection.QueryAsync(sql,
            new
            {
                userActive.IdUsuario,
                userActive.Estado,
            });
        }

        public async Task UpdatedUserAsync(UserUpdatedParemeter user)
        {
            var sql = @"UPDATE Usuario
                        SET UsuarioRolId = @UsuarioRolId,
                        NombreUsuario = @Usuario,
                        Clave = @Password
                        WHERE IDUsuario = @IdUsuario";
            await _dbConnection.QueryAsync(sql,
            new
            {
                user.IdUsuario,
                user.UsuarioRolId,
                user.Usuario,
                user.Password
            });
        }

        public async Task<dynamic> ValidarToken(ClaimsIdentity identity)
        {
            try
            {
                if (!identity.Claims.Any())
                {
                    return new FetchResponse
                    {
                        Success = false,
                        Message = "Verificar si estas enviando un token válido",
                        Data = ""
                    };
                }

                var idClaim = identity.Claims.FirstOrDefault(x => x.Type == "idUsuario");
                if (idClaim == null || string.IsNullOrEmpty(idClaim.Value))
                {
                    return new FetchResponse
                    {
                        Success = false,
                        Message = "No se encontró el claim 'idUsuario' en el token",
                        Data = ""
                    };
                }

                if (!int.TryParse(idClaim.Value, out int id))
                {
                    return new FetchResponse
                    {
                        Success = false,
                        Message = "El claim 'idUsuario' no es un número válido",
                        Data = ""
                    };
                }

                var sql = @"SELECT ISNULL(U.UsuarioRolId, 2) UsuarioRolId
                        FROM Usuario U
                        WHERE U.IDUsuario = @IdUsuario ";
                var result = await _dbConnection.QueryFirstOrDefaultAsync<int?>(sql,
                    new
                    {
                        IdUsuario = idClaim.Value,
                    });
                if (result == null)
                {
                    return new FetchResponse
                    {
                        Success = false,
                        Message = "Usuario no existe",
                        Data = ""
                    };
                }

                return new FetchResponse
                {
                    Success = true,
                    Message = "Token válido",
                    Data = result.Value.ToString()
                };
            }
            catch (Exception ex)
            {
                return new FetchResponse
                {
                    Success = false,
                    Message = "Catch: " + ex.Message,
                    Data = ""
                };
            }
        }
    }
}
