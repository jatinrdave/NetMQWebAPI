using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System;
using WebApplication1;

namespace Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NetMQController : ControllerBase
    {
        private readonly IWindowmakerService _windowmakerService;
        private readonly ILogger<NetMQController> _logger;

        public NetMQController(IWindowmakerService windowmakerService, ILogger<NetMQController> logger)
        {
            _windowmakerService = windowmakerService;
            _logger = logger;
        }

        [HttpPost]
        [Route("Calculation")]
        public async Task<object> CalculateSalesLine(CalculationRequestInput calculationRequestInput) //string calculationRequestInput)
        {
            double value = 0;
            try
            {
                var calculationRequestOutput = await _windowmakerService.InvokeCalculate(calculationRequestInput, AppSettings.OrganisationId, AppSettings.TenantID);

                return Ok(calculationRequestOutput);
            }
            catch (Exception ex)
            {

            }
            return Ok(value);
        }
    }
}