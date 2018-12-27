﻿using System;
using System.Collections.Generic;
using System.Text;
using HotPotato.Models;
using NSwag;

namespace HotPotato.OpenApi.Locators.Default
{
    internal class MethodLocator
    {
        public SwaggerOperation Locate(HttpPair pair, SwaggerPathItem path)
        {
            string method = pair.Request.Method;
            SwaggerOperationMethod operationMethod = toOperationMethod(method);
            return path[operationMethod];
        }

        private SwaggerOperationMethod toOperationMethod(string method)
        {
            switch (method)
            {
                case HttpVerbs.DELETE:
                    return SwaggerOperationMethod.Delete;
                case HttpVerbs.GET:
                    return SwaggerOperationMethod.Get;
                case HttpVerbs.OPTIONS:
                    return SwaggerOperationMethod.Options;
                case HttpVerbs.PATCH:
                    return SwaggerOperationMethod.Patch;
                case HttpVerbs.POST:
                    return SwaggerOperationMethod.Post;
                case HttpVerbs.PUT:
                    return SwaggerOperationMethod.Put;
                case HttpVerbs.TRACE:
                    return SwaggerOperationMethod.Trace;
                default:
                    return SwaggerOperationMethod.Undefined;
            }
        }
    }
}
