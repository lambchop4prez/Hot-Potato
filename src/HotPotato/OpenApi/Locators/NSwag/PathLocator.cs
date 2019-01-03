﻿using System;
using System.Collections.Generic;
using System.Text;
using HotPotato.Matchers;
using HotPotato.Models;
using NSwag;

namespace HotPotato.OpenApi.Locators.NSwag
{
    internal class PathLocator
    {
        public SwaggerPathItem Locate(HttpPair pair, SwaggerDocument swaggerDocument)
        {
            _ = pair ?? throw new ArgumentNullException(nameof(pair));
            _ = swaggerDocument ?? throw new ArgumentNullException(nameof(swaggerDocument));

            string match = PathMatcher.Match(pair.Request.Uri.AbsolutePath, swaggerDocument.Paths.Keys);
            return swaggerDocument.Paths[match];
        }
    }
}
