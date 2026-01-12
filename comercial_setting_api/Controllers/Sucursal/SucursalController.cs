using comercial_setting_api.MessageResult;
using Microsoft.AspNetCore.Mvc;
using setting.Dapper.Sucursal;
using System.Data.Common;

namespace comercial_setting_api.Controllers.Sucursal
{
    [ApiController]
    [Route("api/[controller]")]
    public class SucursalController : ControllerBase
    {
        private readonly ISucursal _sucursal;

        public SucursalController(ISucursal sucursal)
        {
            _sucursal = sucursal;
        }

        [HttpGet("GetAllSucursal")]
        public async Task<IActionResult> GetUserListAsync()
        {
            try
            {
                var ips = await _sucursal.GetSucursalListAsync();
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
    }
}
