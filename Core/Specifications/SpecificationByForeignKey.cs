using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications;

public class SpecificationByForeignKey<T> : BaseSpecification<T> where T : BaseEntity
{
    public SpecificationByForeignKey(int foreignKeyValue, string foreignKeyPropertyName) : base(GenerateCriteria(foreignKeyValue, foreignKeyPropertyName))
    {

    }

    private static Expression<Func<T, bool>> GenerateCriteria(int foreignKeyValue, string foreignKeyPropertyName)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var proterty = Expression.PropertyOrField(parameter, foreignKeyPropertyName);
        var constant = Expression.Constant(foreignKeyValue);
        var body = Expression.Equal(proterty, constant);

        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }
}
