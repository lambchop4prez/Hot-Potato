
using System;
using Microsoft.Extensions.DependencyInjection;
using NSwag;
using Xunit;
using static HotPotato.IntegrationTestMethods;

namespace HotPotato.OpenApi.SpecificationProvider
{
    public class SpecificationProviderTest
    {
        [SkippableFact]
        public void ISpecificationProvider_GetSpecDocument_ReturnsDocumentFromPath()
        {
            string specPath = SpecPath("specs/keyword/", "specification.yaml");
            Skip.If(string.IsNullOrEmpty(specPath), TestConstants.InternalUseOnlyMessage);

            ServiceProvider provider = GetServiceProvider(specPath);

            ISpecificationProvider subject = provider.GetService<ISpecificationProvider>();
            OpenApiDocument result = subject.GetSpecDocument();

            Assert.Equal(result.DocumentPath, specPath);
        }
    }
}
