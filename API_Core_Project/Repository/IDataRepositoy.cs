using API_Core_Project.Models;

namespace API_Core_Project.Repository
{
    public interface IDataRepositoy<TEntity, in TPk> where TEntity : EntityBase
    {
        Task<CollectionResponse<TEntity>> GetAsync();
        Task<SingleObjectResponse<TEntity>> GetAsync(TPk id);
        Task<SingleObjectResponse<TEntity>> CreateAsync(TEntity entity);
        Task<SingleObjectResponse<TEntity>> UpdateAsync(TPk id, TEntity entity);
        Task<SingleObjectResponse<TEntity>> DeleteAsync(TPk id);
    }
}
