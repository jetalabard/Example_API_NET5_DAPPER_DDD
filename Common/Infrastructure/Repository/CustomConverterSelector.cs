using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Common.Domain;
using Common.Domain.BuildingBlocks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Common.Infrastructure.Repository
{
    public class CustomConverterSelector : ValueConverterSelector
    {
        private readonly ConcurrentDictionary<(Type ModelClrType, Type ProviderClrType), ValueConverterInfo> _converters
            = new ConcurrentDictionary<(Type ModelClrType, Type ProviderClrType), ValueConverterInfo>();

        public CustomConverterSelector(ValueConverterSelectorDependencies dependencies)
            : base(dependencies)
        {
        }

        public override IEnumerable<ValueConverterInfo> Select(Type modelClrType, Type providerClrType = null)
        {
            var baseConverters = base.Select(modelClrType, providerClrType);
            foreach (var converter in baseConverters)
            {
                yield return converter;
            }

            var underlyingModelType = UnwrapNullableType(modelClrType);

            var isEnumeration = underlyingModelType.BaseType is not null
                && underlyingModelType.BaseType == typeof(Enumeration);

            var isSingleValueObject = underlyingModelType.BaseType is not null
                 && underlyingModelType.BaseType.IsGenericType
                 && underlyingModelType.BaseType.GetGenericTypeDefinition() == typeof(SingleValueObject<>);

            if (isEnumeration)
            {
                var innerType = typeof(string);
                var converterType = typeof(EnumerationValueConverter<>).MakeGenericType(underlyingModelType);
                yield return Convert(converterType, modelClrType, underlyingModelType, innerType);

            }

            if (isSingleValueObject)
            {
                var innerType = underlyingModelType.BaseType.GetGenericArguments()[0];
                var converterType = typeof(SingleValueObjectConverter<,>).MakeGenericType(underlyingModelType, innerType);
                yield return Convert(converterType, modelClrType, underlyingModelType, innerType);
            }
        }

        private ValueConverterInfo Convert(Type converterType, Type modelClrType, Type underlyingModelType, Type innerType)
        {
            return _converters.GetOrAdd((underlyingModelType, innerType), _ =>
             {
                 return new ValueConverterInfo(
                     modelClrType: modelClrType,
                     providerClrType: innerType,
                     factory: valueConverterInfo => (ValueConverter)Activator.CreateInstance(converterType, valueConverterInfo.MappingHints));
             });
        }

        private static Type UnwrapNullableType(Type type)
        {
            if (type is null)
            {
                return null;
            }

            return Nullable.GetUnderlyingType(type) ?? type;
        }
    }
}