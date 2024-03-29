namespace API_Core_Project.Models
{
    public class ResponseObject<TEntity> where TEntity : class
    {
        public string? Message { get; set; }
        public int StatusCode { get; set; }
    }

    /// <summary>
    /// This call will return collection
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class CollectionResponse<TEntity> : ResponseObject<TEntity> where TEntity : class
    {
        public IEnumerable<TEntity>? Records { get; set; }
    }


    public class SingleObjectResponse<TEntity> : ResponseObject<TEntity> where TEntity : class
    {
        public TEntity? Record { get; set; }
    }
}
