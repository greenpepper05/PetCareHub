using System;
using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications;

public class PetByOwnerIdSpecification : BaseSpecification<Pet>
{
    public PetByOwnerIdSpecification(string onwerId) : base(p => p.OwnerId == onwerId)
    {
    }
    
}
