using System;
using Nap.Html.Serialization.Contracts.Base;

namespace Nap.Html.Serialization.Contracts
{
    public class StringContract : PrimitiveContract
    {
        public StringContract(Type underlyingType) : base(underlyingType)
        {
            ContractType = ContractType.String;
        }
    }
}