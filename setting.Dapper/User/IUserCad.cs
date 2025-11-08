using setting.Shared.DTOs.User;

namespace setting.Dapper.User
{
    public interface IUserCad
    {
        Task<IEnumerable<UserListDto>> GetUserListAsync(bool estado, int idLocal);
        Task<UserListDto> GetOneUserAsync(int idLocal, string user, string password);
    }
}
