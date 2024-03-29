using Assignment_05_03.Models;

namespace Assignment_05_03.Repository
{
   
    public interface IDataRespository<TEntity, in TPk> where TEntity : EntityBase
    {
        Task<CollectionRespons<TEntity>> GetAsync();
        Task<SingleObjectRespons<TEntity>> GetAsync(TPk id);
        Task<SingleObjectRespons<TEntity>> CreateAsync(TEntity entity);
        Task<SingleObjectRespons<TEntity>> UpdateAsync(TPk id, TEntity entity);
        Task<SingleObjectRespons<TEntity>> DeleteAsync(TPk id);

        Task<CollectionRespons<Product>> GetProducts(string CatName, string condition, string manufacturer);

        Task<CollectionRespons<Product>> GetProductsByCategoryName(string CatName);
    }
}
