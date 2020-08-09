using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThermometerWS.Service.ThermometerReader
{
    public interface IThermometerReader
    {
        Task<double?> GetCurrentMeasurementAsync();
    }
}
