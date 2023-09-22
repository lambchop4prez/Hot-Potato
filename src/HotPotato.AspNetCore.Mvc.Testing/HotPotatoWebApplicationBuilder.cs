using HotPotato.AspNetCore.Middleware;
using HotPotato.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace HotPotato.AspNetCore.Mvc.Testing;

public class HotPotatoWebApplicationBuilder<TProgram>
	: WebApplicationFactory<TProgram> where TProgram : class
{

	private readonly string specLocation;
	public HotPotatoWebApplicationBuilder(string specLocation)
	{
		this.specLocation = specLocation ?? throw new ArgumentNullException(nameof(specLocation));
	}
	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.ConfigureServices(services =>
		{
			services.AddHotPotatoServices();

		});
		builder.Configure(app =>
		{
			app.UseMiddleware<HotPotatoMiddleware>();
		});
		builder.ConfigureAppConfiguration(config => {
			config.AddInMemoryCollection(new List<KeyValuePair<string, string>>
			{
				new KeyValuePair<string, string>("SpecLocation", this.specLocation)
			});
		});
		base.ConfigureWebHost(builder);
	}
}
