using comercial_setting_api.MessageResult;
using comercial_setting_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using setting.Dapper.User;
using setting.Shared.DTOs;
using setting.Shared.Structure;
using setting.Shared.Structure.User;
using System.Data.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace comercial_setting_api.Controllers.User
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserCadController : ControllerBase
    {
        private readonly IUserCad _userCad;
        public IConfiguration _configuration;

        public UserCadController(IConfiguration configuration, IUserCad userCad)
        {
            _configuration = configuration;
            _userCad = userCad;
        }

        [HttpGet("GetAllUser")]
        public async Task<IActionResult> GetUserListAsync()
        {
            try
            {
                var users = await _userCad.GetUserListAsync();
                return Ok(ApiResponseHelper.SuccessResponse(users));
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
        [HttpPost("Loginuser")]
        public async Task<IActionResult> GetOneUserAsync(UserLoginParameter userLoginParameter)
        {
            try
            {
                FetchResponse data = await _userCad.GetUserOneAsync(userLoginParameter);
                if (!data.Success)
                {
                    return Conflict(ApiResponseHelper.CustomResponse<object>(data, data.Success, data.Message));
                }
                else
                {
                    var jwt = _configuration.GetSection("Jwt").Get<Jwt>();

                    UserOneDto response = (UserOneDto)data.Data;

                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                        new Claim("idLocal", response.IdLocal.ToString()),
                        new Claim("idUsuario", response.IdUsuario.ToString()),
                        new Claim("usuarioRolId", response.UsuarioRolId.ToString()),
                        new Claim("name", response.Name.ToString()),
                        new Claim("rolName", response.RolName.ToString()),
                    };


                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        jwt.Issuer,
                        jwt.Audience,
                        claims, 
                        expires: DateTime.Now.AddHours(24),
                        signingCredentials: creds
                        );

                    return Ok(ApiResponseHelper.ResponseToken(data.Data, new JwtSecurityTokenHandler().WriteToken(token),  data.Success, data.Message));
                }
            }
            catch (DbException e)
            {
                return BadRequest(ApiResponseHelper.ErrorResponse<object>(e.Message));
            }
            catch (Exception e)
            {
                return BadRequest(ApiResponseHelper.ErrorResponse<object>(e.Message));
            }
        }

        [Authorize]
        [HttpPost("CreatedUser")]
        public async Task<IActionResult> PostCreateUser(UserCreateParameter userCreateParameter)
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                FetchResponse rToten = await _userCad.ValidarToken(identity);

                if (!rToten.Success)
                {
                    return Unauthorized(ApiResponseHelper.CustomResponse(string.Empty, rToten.Success, rToten.Message));
                    // Aquí va la lógica que depende de tokenValidationResult si es necesario
                }
                if (rToten.Data.ToString() != "1")
                {
                    return Unauthorized(ApiResponseHelper.CustomResponse(string.Empty, rToten.Success, rToten.Message));
                }


                FetchResponse data = await _userCad.CreateUserAsync(userCreateParameter);
                if (!data.Success)
                {
                    return Conflict(ApiResponseHelper.CustomResponse<object>(string.Empty, data.Success, data.Message));
                }
                else
                {
                    return Ok(ApiResponseHelper.CustomResponse(data.Data, data.Success, data.Message));
                }
            }
            catch (DbException e)
            {
                return BadRequest(ApiResponseHelper.ErrorResponse<object>(e.Message));
            }
            catch (Exception e)
            {
                return BadRequest(ApiResponseHelper.ErrorResponse<object>(e.Message));
            }
        }
        [HttpPut("UpdatedUser")]
        public async Task<IActionResult> PutUpdateUser(UserUpdatedParemeter userUpdatedParemeter)
        {
            try
            {
                await _userCad.UpdatedUserAsync(userUpdatedParemeter);
                return Ok(ApiResponseHelper.SuccessResponseEmpty<object>());
                
            }
            catch (DbException e)
            {
                return BadRequest(ApiResponseHelper.ErrorResponse<object>(e.Message));
            }
            catch (Exception e)
            {
                return BadRequest(ApiResponseHelper.ErrorResponse<object>(e.Message));
            }
        }
        [HttpPut("UpdatedUserActive")]
        public async Task<IActionResult> PutUpdateUserActive(UserUpdatedActiveParameter userUpdatedActiveParameter)
        {
            try
            {
                await _userCad.UpdatedUserActiveAsync(userUpdatedActiveParameter);
                return Ok(ApiResponseHelper.SuccessResponseEmpty<object>());
            }
            catch (DbException e)
            {
                return BadRequest(ApiResponseHelper.ErrorResponse<object>(e.Message));
            }
            catch (Exception e)
            {
                return BadRequest(ApiResponseHelper.ErrorResponse<object>(e.Message));
            }
        }
    }
}
