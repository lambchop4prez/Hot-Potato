using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace HotPotato.Core.Proxy
{
    public interface IProxy
    {
        Task ProcessAsync(string remoteEndpoint, Microsoft.AspNetCore.Http.HttpRequest requestIn, Microsoft.AspNetCore.Http.HttpResponse responseOut);
    }
}
