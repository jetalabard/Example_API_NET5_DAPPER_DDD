using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Domain.BuildingBlocks
{
    public abstract record ValueObject
    {
        // necessary for EF
        [NotMapped]
        public Guid Id { get; set; }
    }
}
