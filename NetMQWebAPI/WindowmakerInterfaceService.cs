using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1
{
    public class WindowmakerInterfaceService : APIBaseService, IWindowmakerService
    {
        private ConcurrentDictionary<int, IDispatcherService> _serviceDictionary = new ConcurrentDictionary<int, IDispatcherService>();

        public WindowmakerInterfaceService()
        {
        }

        public string GetList(int organization, int parameter)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Invoke calculation method.
        /// </summary>
        /// <param name="calculationRequestInput"></param>
        /// <param name="organization"></param>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        public async Task<string> InvokeCalculate(CalculationRequestInput calculationRequestInput, int organization, int tenantID)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };

            string result = string.Empty;

            _logger?.Info($"{GetType().ToString()}::{MethodBase.GetCurrentMethod().Name} - In");
            _logger?.Info("Calculation Input: {0} at {1}", calculationRequestInput.CalculationInput, DateTime.Now.ToLongTimeString());


            try
            {
                IDispatcherService windowmakerAsService = GetService(organization, tenantID);

                WMServices.Core.Message message = new WMServices.Core.Message() { MethodID = EMethodID.Calculate, Data = calculationRequestInput.CalculationInput };
                string messageString = JsonConvert.SerializeObject(message, typeof(WMServices.Core.Message), settings);
                //Call Calculation
                result = await windowmakerAsService.InvokeWindowmakerService(messageString);
                _logger?.Info("Calculation Output: {0} at {1}", result, DateTime.Now.ToLongTimeString());
            }
            catch (Exception ex)
            {
                result = ex.ToString();
                _logger?.Error("Calculation Error: {0} at {1}", result, DateTime.Now.ToLongTimeString());
            }
            finally
            {
                _logger?.Info($"{GetType().ToString()}::{MethodBase.GetCurrentMethod().Name} - Out");
            }
            return result;
        }
        /// <summary>
        /// Get Service from the service dictionary.
        /// </summary>
        /// <param name="organization"></param>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        private IDispatcherService GetService(int organization, int tenantID)
        {

            if (!_serviceDictionary.ContainsKey(organization))
            {

                _logger?.Info($"{GetType().ToString()}::{MethodBase.GetCurrentMethod().Name} - In");

                int PortStartAddress = 5000;
                 int portNo = PortStartAddress + Convert.ToInt32(organization);
                //var port = ConfigurationManager.AppSettings[organization];
                //int portNo = 5000 + Convert.ToInt32(organization);
                //ILoggerFactory loggerFactory = AutofacConfigService.GetLifetimeScope().Resolve<ILoggerFactory>();
                IDispatcherService wmDispatcherService = new WMDispatcherService();// AutofacConfigService.GetLifetimeScope().Resolve<IDispatcherService>();
                _logger?.Info($"Initialising Service for organization {organization} tenantID {tenantID} on {portNo}");
                wmDispatcherService.Initialize(portNo.ToString());
                _logger?.Info($"Initialise Service completed ");
                _serviceDictionary.TryAdd(organization, wmDispatcherService);
                _logger?.Info($"Windowmaker Service executable Spawn");
            }
            _logger?.Info($"{GetType().ToString()}::{MethodBase.GetCurrentMethod().Name} - Out");
            return _serviceDictionary[organization];

        }
        /// <summary>
        /// Initialise services e.g. Cache.
        /// </summary>
        /// <param name="organisationId"></param>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        public async Task InitService(int organisationId, int tenantID)
        {
            _logger?.Info($"{GetType().ToString()}::{MethodBase.GetCurrentMethod().Name} - In");
            _logger?.Info("First Run at {0}", DateTime.Now.ToLongTimeString());
            //Initialise only.
            IDispatcherService windowmakerDispatcherService = GetService(organisationId, tenantID);
        }
    }
}
