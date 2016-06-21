using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Resources;
using FluentValidation.Results;
using System.Threading;

namespace WebApi.Domain.Extensions
{
    public static class FluentValidationExtensions
    {
        public static IRuleBuilderOptions<TModel, TProperty> Rule<TModel, TProperty>(this IRuleBuilder<TModel, TProperty> builder, Func<TProperty, bool> validator)
        {
            return builder.SetValidator(new InlineValidator<TProperty>(validator));
        }

        private class InlineValidator<TProperty> : PropertyValidator
        {
            private Func<TProperty, bool> _validator;
            public InlineValidator(Func<TProperty, bool> validator) : base("Failed Validation")
            {
                _validator = validator;
            }

            protected override bool IsValid(PropertyValidatorContext context)
            {
                var value = context.PropertyValue;

                if (!(value is TProperty))
                    value = default(TProperty);

                return _validator((TProperty)value);
            }
        }
    }
}
