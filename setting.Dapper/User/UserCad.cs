using Dapper;
using setting.Shared.DTOs.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace setting.Dapper.User
{
    public class UserCad : IUserCad
    {
        private readonly IDbConnection _dbConnection;

        public UserCad(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
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
            var value = (result != null) ? result : null;
            return value;


        }

        public async Task<IEnumerable<UserListDto>> GetUserListAsync(bool estado, int idLocal)
        {

            var sqlAll = @"SELECT U.IDUsuario IdUsuario, ISNULL(U.UsuarioRolId, -1) UsuarioRolId, L.IDLocal IdLocal, ISNULL(UR.Descripcion, 'ERROR' ) Rol, L.[Local] Sucursal, U.NombreUsuario Usuario, U.Estado
                FROM Usuario U
                LEFT JOIN Usuario_Rol UR ON U.UsuarioRolId = UR.UsuarioRolId
                INNER JOIN Local L ON U.IDLocal = L.IDLocal
                ORDER BY U.NombreUsuario ASC";
            var sqlLocal = @"SELECT U.IDUsuario IdUsuario, ISNULL(U.UsuarioRolId, -1) UsuarioRolId, L.IDLocal IdLocal, ISNULL(UR.Descripcion, 'ERROR' ) Rol, L.[Local] Sucursal, U.NombreUsuario Usuario, U.Estado
                FROM Usuario U
                LEFT JOIN Usuario_Rol UR ON U.UsuarioRolId = UR.UsuarioRolId
                INNER JOIN Local L ON U.IDLocal = L.IDLocal
                WHERE U.IDLocal = @IdLocal
                ORDER BY U.NombreUsuario ASC";
            IEnumerable<UserListDto> result;
            if (estado)
            {
                result = await _dbConnection.QueryAsync<UserListDto>(sqlAll);
            }
            else
            {
                result = await _dbConnection.QueryAsync<UserListDto>(sqlLocal,
                new
                {
                    IdLocal = idLocal
                });
            }

            return result;

        }
    }
}
