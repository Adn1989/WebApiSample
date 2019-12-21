using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Core.Models;
using Web.Api.Core.ViewModels;
using Web.Api.Infrastructure.Interfaces;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PointDistanceController : ControllerBase
    {
        private readonly IDistanceCalculateService _distanceCalculateService;

        public PointDistanceController(IDistanceCalculateService distanceCalculateService)
        {
            _distanceCalculateService = distanceCalculateService;
        }

        // Post: api/pointdistance/calculate
        [HttpPost("calculate")]
        public ActionResult<double> Calculate([FromForm]DistanceCalculateVM distanceCalculate)
        {
            try
            {
                var _appUserId = User.FindFirst(ClaimTypes.PrimarySid)?.Value;
                if (string.IsNullOrEmpty(_appUserId))
                {
                    return BadRequest("امکان شناسایی کاربر وجود ندارد");
                }

                int appUserId = int.Parse(_appUserId);

                double distance = _distanceCalculateService.Calculate(appUserId, distanceCalculate);

                return Ok($"مسافت {distance} کیلومتر");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // Post: api/pointdistance/userlogs
        [HttpPost("userlogs")]
        public async Task<ActionResult<IEnumerable<AppUserCalculateLog>>> GetUserLogs([FromForm]int appUserId)
        {
            try
            {
                IList<AppUserCalculateLog> calculateLogs = await _distanceCalculateService.GetCalculateLog(appUserId);

               return Ok(calculateLogs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}