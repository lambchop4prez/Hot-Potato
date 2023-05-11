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
        var subject = new HotPotatoWebApplicationBuilder<Test.Api.Startup>();
    }
}

