namespace HotPotato.AspNetCore.Mvc.Testing;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        var builder = new HotPotatoWebApplicationBuilder<Test.Api.Startup>();
        var client = builder.CreateClient();
        var response = client.GetAsync("/orders");
        
    }
}

