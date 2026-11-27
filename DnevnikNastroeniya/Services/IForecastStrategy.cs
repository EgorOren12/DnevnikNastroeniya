using System.Threading.Tasks;
using DnevnikNastroeniya.Models;

namespace DnevnikNastroeniya.Services
{
    internal interface IForecastStrategy
    {
        Task<Forecast> GenerateForecastAsync(int clientId);
    }
}