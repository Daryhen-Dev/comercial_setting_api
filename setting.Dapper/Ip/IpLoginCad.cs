using Dapper;
using setting.Shared.DTOs;
using setting.Shared.Structure.Ip;
using System.Data;

namespace setting.Dapper.Ip
{
    public class IpLoginCad : IIpLoginCad
    {
        private readonly IDbConnection _dbConnection;

        public IpLoginCad(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<IpLoginDto>> GetIpLoginListAsync()
        {
            var sql = @"SELECT AjusteIpSucursalId, AIS.IdLocal, L.Local Sucursal, AIS.Detalle, AIS.Ip, AIS.DateCreate, AIS.DateUpdate
                        FROM AjusteIpSucursal AIS
                        INNER JOIN Local L on AIS.IdLocal = L.IDLocal
                        ORDER BY L.Local , AIS.Detalle ASC";
            var result = await _dbConnection.QueryAsync<IpLoginDto>(sql);
            return result;
        }

        public async Task UpdateIpLocal(IpLoginUpdatedParameter ipLoginUpdatedParameter)
        {
            var sql = @"UPDATE AjusteIpSucursal
                        SET Detalle = @Detalle,
                        Ip = @Ip,
                        DateUpdate = GETDATE()
                        WHERE AjusteIpSucursalId = @AjusteIpSucursalId";
            await _dbConnection.QueryAsync(sql, new
            {
                ipLoginUpdatedParameter.AjusteIpSucursalId,
                ipLoginUpdatedParameter.Detalle,
                ipLoginUpdatedParameter.Ip
            });
        }
    }
}
