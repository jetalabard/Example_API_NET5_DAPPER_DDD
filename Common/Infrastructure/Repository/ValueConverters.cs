using System;
using System.Linq;
using Common.Domain;
using Common.Domain.BuildingBlocks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Common.Infrastructure.Repository
{
    public class SingleValueObjectConverter<TTypedIdValue, T> : ValueConverter<TTypedIdValue, T>
        where TTypedIdValue : SingleValueObject<T>
    {
        public SingleValueObjectConverter(ConverterMappingHints mappingHints = null)
            : base(vo => vo.Value, value => Create(value), mappingHints)
        {
        }

        private static TTypedIdValue Create(T value) => Activator.CreateInstance(typeof(TTypedIdValue), value) as TTypedIdValue;
    }

    public class EnumerationValueConverter<TEnumeration> : ValueConverter<TEnumeration, string>
        where TEnumeration : Enumeration
    {
        public EnumerationValueConverter(ConverterMappingHints mappingHints = null)
            : base(e => e.Code, value => Create(value), mappingHints)
        {
        }

        private static TEnumeration Create(string code) => Enumeration.GetAll<TEnumeration>().Single(e => e.Code == code);
    }
}
