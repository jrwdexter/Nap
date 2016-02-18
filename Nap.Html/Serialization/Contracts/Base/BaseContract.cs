using System;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Nap.Html.Serialization.Contracts.Base
{
    public abstract class BaseContract
    {
        private readonly Type _underlyingType;
        private bool _isNullable;
        private bool _isEnum;
        private Type _nonNullableUnderlyingType;
        private Type _createdType;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseContract"/> class.
        /// </summary>
        /// <param name="underlyingType">Type of the underlying object for this contract.</param>
        protected BaseContract(Type underlyingType)
        {
            if (underlyingType == null)
                throw new ArgumentNullException(nameof(underlyingType));
            _underlyingType = underlyingType;
            _isNullable = !underlyingType.IsValueType || IsNullableType(underlyingType);
            _nonNullableUnderlyingType = IsNullableType(underlyingType)
                                         ? Nullable.GetUnderlyingType(underlyingType)
                                         : underlyingType;
            CreatedType = _nonNullableUnderlyingType;
        }

        private static bool IsNullableType(Type underlyingType)
        {
            return underlyingType.IsGenericType && underlyingType.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public bool IsSealed { get; set; }
        public bool IsInstantiable { get; set; }
        public bool IsReference { get; set; }
        public bool IsNullable => _isNullable;
        public bool IsEnum => _isEnum;
        public bool HasDefaultConstructor => DefaultConstructor != null;
        public ConstructorInfo DefaultConstructor { get; set; }

        public ContractType ContractType { get; set; }

        /// <summary>
        /// Gets the type of the object that is 
        /// </summary>
        public Type UnderlyingType => _underlyingType;

        /// <summary>
        /// Gets or sets the type of the object created after deserialization.
        /// </summary>
        public Type CreatedType
        {
            get { return _createdType; }
            private set
            {
                _createdType = value;
                IsSealed = _createdType.IsSealed;
                IsInstantiable = !(_createdType.IsInterface || _createdType.IsAbstract);
            }
        }

        /// <summary>
        /// Gets the type of the non nullable underlying type. This will either be the type itself or T if the type is <see cref="Nullable{T}"></see>
        /// </summary>
        protected Type NonNullableUnderlyingType => _nonNullableUnderlyingType;
    }
}
