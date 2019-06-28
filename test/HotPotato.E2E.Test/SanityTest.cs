﻿using HotPotato.Core.Http;
using HotPotato.Core.Http.Default;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using WireMock.Server;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using Xunit;

namespace HotPotato.E2E.Test
{
    [Collection("Host")]
    public class SanityTest
    {
        private IWebHost host;

        private const string ApiLocation = "http://localhost:5000";
        private const string Endpoint = "/endpoint";
        private const string ProxyEndpoint = "http://localhost:3232/endpoint";
        private const string GetMethodCall = "GET";
        private const string OKResponseMessage = "OK";
        private const string NotFoundResponseMessage = "Not Found";
        private const string InternalServerErrorResponseMessage = "Internal Server Error";
        private const string PlainTextContentType = "text/plain";
        private const string ApplicationJsonContentType = "application/json";
        private const string ContentType = "Content-Type";

        public SanityTest(HostFixture fixture)
        {
            host = fixture.host;
        }

        [Fact]
        public async Task HotPotato_Should_Return_OK_And_A_String()
        {
            var servicePro = host.Services;
            //test if the right spec location is in the test config
            Assert.Equal(HostFixture.SpecLocation, servicePro.GetService<IConfiguration>()["SpecLocation"]);

            //Setting up mock server to hit
            const string expected = "ValidResponse";

            using (var server = FluentMockServer.Start(ApiLocation))
            {
                server
                    .Given(
                        Request.Create()
                            .WithPath(Endpoint)
                            .UsingGet()
                    )
                    .RespondWith(
                        Response.Create()
                            .WithStatusCode(200)
                            .WithHeader(ContentType, PlainTextContentType)
                            .WithBody(expected)
                    );

                Core.Http.Default.HttpClient client = (Core.Http.Default.HttpClient)servicePro.GetService<IHttpClient>();

                HttpMethod method = new HttpMethod(GetMethodCall);

                using (HttpRequest req = new HttpRequest(method, new System.Uri(ProxyEndpoint)))
                {
                    IHttpResponse res = await client.SendAsync(req);
                    
                    //Asserts
                    Assert.Equal(HttpStatusCode.OK, res.StatusCode);
                    Assert.Equal(PlainTextContentType, res.ContentType.Type);
                    Assert.Equal(expected, res.ToBodyString());
                }
            }
        }

        [Fact]
        public async Task HotPotato_Should_Return_OK_And_A_JSON_Object()
        {
            var servicePro = host.Services;

            //Setting up mock server to hit
            string json = @"{
                'Email': 'james@example.com',
                'Active': true,
                'CreatedDate': '2013-01-20T00:00:00Z',
                'Roles': [
                    'User',
                    'Admin'
                ]}";

            using (var server = FluentMockServer.Start(ApiLocation))
            {
                server
                    .Given(
                        Request.Create()
                            .WithPath(Endpoint)
                            .UsingGet()
                    )
                    .RespondWith(
                        Response.Create()
                            .WithStatusCode(200)
                            .WithHeader(ContentType, ApplicationJsonContentType)
                            .WithBody(json)
                    );

                Core.Http.Default.HttpClient client = (Core.Http.Default.HttpClient)servicePro.GetService<IHttpClient>();

                HttpMethod method = new HttpMethod(GetMethodCall);

                using (HttpRequest req = new HttpRequest(method, new System.Uri(ProxyEndpoint)))
                {

                    IHttpResponse res = await client.SendAsync(req);

                    //Asserts
                    Assert.Equal(HttpStatusCode.OK, res.StatusCode);
                    Assert.Equal(ApplicationJsonContentType, res.ContentType.Type);
                    Assert.Equal(json, res.ToBodyString());
                }
            }
        }

        [Fact]
        public async Task HotPotato_Should_Return_404_Error()
        {
            var servicePro = host.Services;

            using (var server = FluentMockServer.Start(ApiLocation))
            {
                server
                    .Given(
                        Request.Create()
                            .WithPath(Endpoint)
                            .UsingGet()
                    )
                    .RespondWith(
                        Response.Create()
                            .WithStatusCode(404)
                            .WithBody(NotFoundResponseMessage)
                    );


                Core.Http.Default.HttpClient client = (Core.Http.Default.HttpClient)servicePro.GetService<IHttpClient>();
                HttpMethod method = new HttpMethod(GetMethodCall);

                using (HttpRequest req = new HttpRequest(method, new System.Uri(ProxyEndpoint)))
                {

                    IHttpResponse res = await client.SendAsync(req);

                    //Asserts
                    Assert.Equal(HttpStatusCode.NotFound, res.StatusCode);
                    Assert.Equal(NotFoundResponseMessage, res.ToBodyString());
                }
            }
        }

        [Fact]
        public async Task HotPotato_Should_Return_500_Error()
        {
            var servicePro = host.Services;

            using (var server = FluentMockServer.Start(ApiLocation))
            {
                server
                    .Given(
                        Request.Create()
                            .WithPath(Endpoint)
                            .UsingGet()
                    )
                    .RespondWith(
                        Response.Create()
                            .WithStatusCode(500)
                            .WithBody(InternalServerErrorResponseMessage)
                    );


                //Setting up Http Client
                Core.Http.Default.HttpClient client = (Core.Http.Default.HttpClient)servicePro.GetService<IHttpClient>();
                HttpMethod method = new HttpMethod(GetMethodCall);

                using (HttpRequest req = new HttpRequest(method, new System.Uri(ProxyEndpoint)))
                {

                    IHttpResponse res = await client.SendAsync(req);

                    //Asserts
                    Assert.Equal(HttpStatusCode.InternalServerError, res.StatusCode);
                    Assert.Equal(InternalServerErrorResponseMessage, res.ToBodyString());
                }
            }
        }
    }
}
