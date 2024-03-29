namespace Assignment_05_03.Models
{
    public class ResponseObjecte<TEntity> where TEntity : class
    {
        public string? Message { get; set; }
        public int StatusCode { get; set; }
    }

    /// <summary>
    /// This call will return collection
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class CollectionRespons<TEntity> : ResponseObjecte<TEntity> where TEntity : class
    {
        public IEnumerable<TEntity>? Records { get; set; }
    }


    public class SingleObjectRespons<TEntity> : ResponseObjecte<TEntity> where TEntity : class
    {
        public TEntity? Record { get; set; }
    }
}
