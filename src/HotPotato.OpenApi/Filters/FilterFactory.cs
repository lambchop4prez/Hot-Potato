using System.Collections.Generic;
using NJsonSchema;

namespace HotPotato.OpenApi.Filters
{
    public static class FilterFactory
    {
        public static List<IValidationErrorFilter> CreateApplicableFilters(JsonSchema schema, string body)
        {
            List<IValidationErrorFilter> filters = new List<IValidationErrorFilter>();
            if (body.Contains("null"))
            {
                filters.Add(new NullableValidationErrorFilter(schema, body));
            }
            //new filters with applicable conditions can be added here
            return filters;
        }
    }
}
