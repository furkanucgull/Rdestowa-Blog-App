namespace App.Data.Abstract;

public interface IRepository<T> where T : class
{
    Task<OperationResult<T>> GetById(int? id);
    IQueryable<T> GetAll();
    Task<OperationResult<T>> Add(T entity);
    Task<OperationResult<T>> Update(T entity);
    Task<OperationResult<T>> Delete(T entity);
}
