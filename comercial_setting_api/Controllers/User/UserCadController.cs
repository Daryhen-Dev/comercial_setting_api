using comercial_setting_api.MessageResult;
using Microsoft.AspNetCore.Mvc;
using setting.Dapper.User;
using System.Data.Common;

namespace comercial_setting_api.Controllers.User
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserCadController : ControllerBase
    {
        private readonly IUserCad _userCad;

        public UserCadController(IUserCad userCad)
        {
            _userCad = userCad;
        }

        [HttpGet("GetAllUser")]
        public async Task<IActionResult> GetUserListAsync([FromQuery] bool estado, [FromQuery] int idLocal)
        {
            try
            {
                var users = await _userCad.GetUserListAsync(estado, idLocal);
                return Ok(users);
            }
            catch (DbException dbException)
            {
                return BadRequest(dbException.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
        [HttpGet("GetOneUser")]
        public async Task<IActionResult> GetOneUserAsync([FromQuery] int idLocal, [FromQuery] string user, [FromQuery] string password)
        {
            try
            {
               
                var Login = await _userCad.GetOneUserAsync(idLocal, user, password);
                if (Login == null)
                {
                    
                    return Ok(ApiResponseHelper.SuccessResponse(Login, "Usuario o contraseña incorrectos."));
                }
                else
                {
                    return Ok(ApiResponseHelper.SuccessResponse(Login));
                }
            }
            catch (DbException dbException)
            {
                return BadRequest(ApiResponseHelper.ErrorResponse<object>(dbException.Message));
            }
            catch (Exception e)
            {
                return BadRequest(ApiResponseHelper.ErrorResponse<object>(e.Message));
            }
        }
    }
}
