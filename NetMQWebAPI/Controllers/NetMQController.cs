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
        private readonly ICalcService _calcService;
        private readonly ILogger<NetMQController> _logger;

        public NetMQController(ICalcService calcService, ILogger<NetMQController> logger)
        {
            _calcService = calcService;
            _logger = logger;
        }

        [HttpPost]
        [Route("Calculation")]
        public async Task<object> CalculateSalesLine(CalculationRequestInput calculationRequestInput) //string calculationRequestInput)
        {
            double value = 0;
            try
            {
                var calculationRequestOutput = await _calcService.InvokeCalculate(calculationRequestInput, AppSettings.OrganisationId, AppSettings.TenantID);

                return Ok(calculationRequestOutput);
            }
            catch (Exception ex)
            {

            }
            return Ok(value);
        }
    }
}