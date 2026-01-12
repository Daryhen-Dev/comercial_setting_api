using comercial_setting_api.MessageResult;
using Microsoft.AspNetCore.Mvc;
using setting.Dapper.Ip;
using System.Data.Common;
using setting.Shared.Structure.Ip;

namespace comercial_setting_api.Controllers.Ip
{
    [ApiController]
    [Route("[controller]")]
    public class IpLoginController : ControllerBase
    {

        private readonly IIpLoginCad _ipLoginCad;

        public IpLoginController(IIpLoginCad ipLoginCad)
        {
            _ipLoginCad = ipLoginCad;
        }

        [HttpGet("GetAllIpLogin")]
        public async Task<IActionResult> GetUserListAsync()
        {
            try
            {
                var ips = await _ipLoginCad.GetIpLoginListAsync();
                return Ok(ApiResponseHelper.SuccessResponse(ips));
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

        [HttpPut("UpdatedIpLogin")]
        public async Task<IActionResult> PutProductActive(IpLoginUpdatedParameter ipLoginUpdatedParameter)
        {
            try
            {
                await _ipLoginCad.UpdateIpLocal(ipLoginUpdatedParameter);
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
