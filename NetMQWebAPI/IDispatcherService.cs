using System.Threading.Tasks;

namespace WebApplication1
{
    public interface IDispatcherService
    {
        void Initialize(string tcpPort);
        Task<string> InvokeWindowmakerService(string messageInput);
    }
}