using System;
using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications;

public class PetByOwnerIdSpec : BaseSpecification<Pet>
{
    public PetByOwnerIdSpec(string onwerId) : base(o => o.OwnerId == onwerId)
    {
        ApplyDisctinct();
    }
}
