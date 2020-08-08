using System.Threading.Tasks;

namespace SlackNotifierWS.Service
{
    public interface ISlackNotifierService
    {
        public Task NotifyAsync(string url, string message);
    }
}