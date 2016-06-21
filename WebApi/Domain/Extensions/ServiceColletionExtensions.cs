using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Domain.Extensions
{
    public static class ServiceColletionExtensions
    {
        public static IServiceCollection AddTransients(this IServiceCollection collection, IEnumerable<Type> types, Func<Type, Type> implementationMapping)
        {
            return Add(collection, types, implementationMapping, collection.AddTransient);
        }

        private static IServiceCollection Add(IServiceCollection collection, IEnumerable<Type> types, Func<Type, Type> mapping, Func<Type, Type, IServiceCollection> adder)
        {
            foreach(Type t in types)
            {
                Type i = mapping(t);

                if (i != null)
                    adder(t, i);
            }

            return collection;
        }
    }
}
