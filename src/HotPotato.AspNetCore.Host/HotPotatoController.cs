using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HotPotato.AspNetCore.Middleware;
using HotPotato.Core;
using HotPotato.Core.Proxy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HotPotato.AspNetCore.Host
{
    [Controller]
    public class HotPotatoController : Controller
    {
        private const string RemoteEndpointKey = "RemoteEndpoint";

        public IProxy Proxy { get; }
        public IConfiguration Configuration { get; }
        public ILogger<HotPotatoController> Log { get; }
        public string RemoteEndpoint { get; }

        public HotPotatoController(IProxy proxy, IConfiguration configuration, ILogger<HotPotatoController> log)
        {
            this.Proxy = proxy ?? throw Exceptions.ArgumentNull(nameof(proxy));
            this.Configuration = configuration ?? throw Exceptions.ArgumentNull(nameof(configuration));
            this.Log = log ?? throw Exceptions.ArgumentNull(nameof(log));

            this.RemoteEndpoint = this.Configuration[RemoteEndpointKey] ?? throw Exceptions.MissingConfigKey(RemoteEndpointKey);


        }

        [ActionName("Process")]
        public async Task Process()
        {
            try
            {
                Log.LogDebug($"{this.Request.Method} {this.Request.Path}");
                await Proxy.ProcessAsync(this.RemoteEndpoint, this.Request, this.Response);

                Log.LogDebug($"{this.Response.StatusCode} Length: {this.Response.ContentLength}");
                Log.LogDebug("--------------- Request End ---------------");
            }
            catch (HttpRequestException httpEx)
            {
                Log.LogError(httpEx, "Failed to forward request. Remote endpoint may be down.");
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadGateway;
            }
            catch (AggregateException agex)
            {
                SpecNotFoundException spex = agex.InnerException as SpecNotFoundException;
                if (spex != null)
                {
                    Log.LogError(agex.InnerException, $"Failed to retrieve spec - please recheck SpecLocation and SpecToken.{Environment.NewLine}StatusCode: {(int)spex.Response.StatusCode}{Environment.NewLine}ReasonPhrase: {spex.Response.ReasonPhrase}");
                }
                else
                {
                    //an example edge case would be a non-existent domain like https://raw.fakegithubusercontent.com/HylandSoftware/Hot-Potato/master/test/RawPotatoSpec.yaml
                    Log.LogError(agex, "Exception thrown from an async call");
                }
                //we'll probably want to set to context.Reponse status code in the future,
                //but for now trying to set it here will cause a "Response already started")
            }
            catch (Exception e)
            {
                //handle unknown exceptions
                Log.LogError(e, "Failed to forward request");
                HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
        }
    }
}

