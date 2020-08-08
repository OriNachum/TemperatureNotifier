using System.Threading.Tasks;

namespace ThermometerWS.Service
{
    public interface IThermometerService
    {
        Task<double> GetTempratureAsync();
    }
}