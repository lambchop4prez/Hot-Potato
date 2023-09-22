namespace HotPotato.AspNetCore.Mvc.Testing;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task Test1()
    {
        var builder = new HotPotatoWebApplicationBuilder<Test.Api.Startup>("https://raw.githubusercontent.com/lambchop4prez/Hot-Potato/main/test/RawPotatoSpec.yaml");
        var client = builder.CreateClient();
        var response = await client.GetAsync("/orders");

    }
}

