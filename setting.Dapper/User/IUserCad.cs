
using setting.Shared.DTOs;
using setting.Shared.Structure;
using setting.Shared.Structure.User;
using System.Security.Claims;

namespace setting.Dapper.User
{
    public interface IUserCad
    {
        Task<IEnumerable<UserListDto>> GetUserListAsync();
        Task<int> GetUserExistAsync(int idUser);
        Task<FetchResponse> GetUserOneAsync(UserLoginParameter userLoginParameter);
        Task<UserListDto> GetOneUserAsync(int idLocal, string user, string password);
        Task UpdatedUserAsync(UserUpdatedParemeter user);
        Task UpdatedUserActiveAsync(UserUpdatedActiveParameter userActive);
        Task<FetchResponse> CreateUserAsync(UserCreateParameter newUser);
        Task<dynamic> ValidarToken(ClaimsIdentity identity);
    }
}
