using System;
using Nap.Html.Serialization.Contracts.Base;

namespace Nap.Html.Serialization.Contracts
{
    public class PrimitiveContract : BaseContract
    {
        internal TypeCode TypeCode { get; set; }

        public PrimitiveContract(Type underlyingType) : base(underlyingType)
        {
            ContractType = ContractType.Primitive;

            TypeCode = Convert.GetTypeCode(NonNullableUnderlyingType);
        }
    }
}