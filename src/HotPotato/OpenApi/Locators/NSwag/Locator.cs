﻿using System;
using System.Collections.Generic;
using System.Text;
using HotPotato.Exceptions;
using HotPotato.Models;
using HotPotato.Validators;
using HotPotato.Validators.NJsonSchema;
using NJsonSchema;
using NSwag;

namespace HotPotato.OpenApi.Locators.NSwag
{
    internal class Locator : ILocator
    {
        private readonly SwaggerDocument swaggerDocument;
        private readonly PathLocator pathLocator;
        private readonly MethodLocator methodLocator;
        private readonly StatusCodeLocator statusCodeLocator;

        public Locator(SwaggerDocument document, PathLocator pathLocator, MethodLocator methodLocator, StatusCodeLocator statusCodeLocator)
        {
            _ = document ?? throw new ArgumentNullException(nameof(document));
            _ = pathLocator ?? throw new ArgumentNullException(nameof(pathLocator));
            _ = methodLocator ?? throw new ArgumentNullException(nameof(methodLocator));
            _ = statusCodeLocator ?? throw new ArgumentNullException(nameof(statusCodeLocator));

            this.swaggerDocument = document;
            this.pathLocator = pathLocator;
            this.methodLocator = methodLocator;
            this.statusCodeLocator = statusCodeLocator;
        }

        public Tuple<IBodyValidator, IHeaderValidator> GetValidator(HttpPair pair)
        {
            SwaggerPathItem path = this.pathLocator.Locate(pair, this.swaggerDocument);
            _ = path ?? throw new LocatorException();
            SwaggerOperation operation = this.methodLocator.Locate(pair, path);
            _ = operation ?? throw new LocatorException();
            SwaggerResponse response = this.statusCodeLocator.Locate(pair, operation);
            _ = response ?? throw new LocatorException();
            JsonSchema4 schema = response.ActualResponseSchema;
            return new Tuple<IBodyValidator, IHeaderValidator>(new BodyValidator(schema), new HeaderValidator(response.Headers));
        }
    }
}
