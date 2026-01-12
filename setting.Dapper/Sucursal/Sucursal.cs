using Dapper;
using setting.Shared.DTOs;
using System.Data;

namespace setting.Dapper.Sucursal
{
    public class Sucursal : ISucursal
    {
        private readonly IDbConnection _dbConnection;

        public Sucursal(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public async Task<IEnumerable<SucursalListDto>> GetSucursalListAsync()
        {
            var sql = @"SELECT L.IDLocal, L.Local Name
                        FROM Local L
                        WHERE L.IDLocal != 2";
            var result = await _dbConnection.QueryAsync<SucursalListDto>(sql);
            if (result == null)
            {
                return Enumerable.Empty<SucursalListDto>();
            }
            else
            {
                return result;
            }
        }
    }
}
