using System.Threading.Tasks;

namespace ThermalNotifierWS.Service
{
    public interface IThermalNotifierService
    {
        Task AlertTemperatureAsync();

        Task<bool> NotifyTemperatureAsync();
    }
}