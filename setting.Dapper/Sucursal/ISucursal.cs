using setting.Shared.DTOs;

namespace setting.Dapper.Sucursal
{
    public interface ISucursal
    {

        Task<IEnumerable<SucursalListDto>> GetSucursalListAsync();

    }
}
