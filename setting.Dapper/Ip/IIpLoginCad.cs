using setting.Shared.DTOs;
using setting.Shared.Structure.Ip;

namespace setting.Dapper.Ip
{
    public interface IIpLoginCad
    {

        Task<IEnumerable<IpLoginDto>> GetIpLoginListAsync();
        Task UpdateIpLocal(IpLoginUpdatedParameter ipLoginUpdatedParameter);
    }
}
