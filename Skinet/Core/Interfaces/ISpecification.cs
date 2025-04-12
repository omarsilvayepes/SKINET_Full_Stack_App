using System.Linq.Expressions;

namespace Core.Interfaces
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>>? Criteria { get; }
        Expression<Func<T,object>>? OrderBy { get; }
        Expression<Func<T, object>>? OrderByDescending { get; }
        bool IsDistinct { get; }
        int Take {  get; }
        int Skip { get; }
        bool IsPagingEnabled {  get; }
        IQueryable<T> ApplyCriteria(IQueryable<T> query);
    }


    //interface use for return an type diferent to the type T
    public interface ISpecification<T, TResult> : ISpecification<T>
    {
        Expression<Func<T, TResult>>? select { get; }
    }
}
