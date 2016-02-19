using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Nap.Html.Serialization.Contracts.Base;

namespace Nap.Html.Serialization.Contracts
{
    public class ObjectContract : BaseContract
    {
        public ConstructorInfo ParameterizedConstructor { get; set; }

        public ICollection<ParameterInfo> Parameters { get; set; }

        public ObjectContract(Type underlyingType) : base(underlyingType)
        {
            ContractType = ContractType.Object;
            DefaultConstructor =
                NonNullableUnderlyingType.GetConstructors(BindingFlags.Instance | BindingFlags.Public)
                    .FirstOrDefault(c => c.GetParameters().Length == 0);
            if (DefaultConstructor == null)
            {
                ParameterizedConstructor =
                    NonNullableUnderlyingType.GetConstructors(BindingFlags.Instance | BindingFlags.Public)
                        .OrderBy(c => c.GetParameters().Length)
                        .FirstOrDefault();

                if (ParameterizedConstructor == null)
                    throw new Exceptions.NapBindingException($"Could not locate a constructor on type {NonNullableUnderlyingType.FullName}");
            }

        }
    }
}