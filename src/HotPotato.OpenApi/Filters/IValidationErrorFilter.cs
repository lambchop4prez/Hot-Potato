using System.Collections.Generic;
using HotPotato.OpenApi.Validators;

namespace HotPotato.OpenApi.Filters
{
    public interface IValidationErrorFilter
    {
        void Filter(IList<ValidationError> validationErrors);
    }
}
