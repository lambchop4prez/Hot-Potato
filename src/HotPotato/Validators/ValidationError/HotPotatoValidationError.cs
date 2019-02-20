﻿using NJsonSchema.Validation;

namespace HotPotato.Validators
{
    public class ValidationError
    {
        public string Message { get; set; }
        public string Kind { get; set; }
        public string Property { get; set; }
        public int LineNumber { get; set; }
        public int LinePosition { get; set; }

        public ValidationError(string message, string kind, string property, int lineNumber, int linePosition)
        {
            this.Message = message;
            this.Kind = kind;
            this.Property = property;
            this.LineNumber = lineNumber;
            this.LinePosition = linePosition;
        }

    }
}
