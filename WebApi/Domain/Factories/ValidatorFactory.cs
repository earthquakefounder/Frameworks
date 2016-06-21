using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace WebApi.Domain.Factories
{
    public class ValidatorFactory : ValidatorFactoryBase
    {
        private IServiceProvider _provider;
        public ValidatorFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public override IValidator CreateInstance(Type validatorType)
        {
            return _provider.GetService(validatorType) as IValidator;
        }
    }
}
