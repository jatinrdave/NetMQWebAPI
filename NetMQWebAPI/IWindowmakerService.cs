using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1
{
    public interface IWindowmakerService
    {
        Task<string> InvokeCalculate(CalculationRequestInput calculationRequestInput, int organization, int tenantID);
       
        Task InitService(int organisationId, int tenantID);
    }
}
