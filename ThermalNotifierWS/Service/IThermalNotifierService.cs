using System.Threading.Tasks;

namespace ThermalNotifierWS.Service
{
    public interface IThermalNotifierService
    {
        Task<bool> AlertTemperatureAsync();

        Task<bool> NotifyTemperatureAsync();
    }
}