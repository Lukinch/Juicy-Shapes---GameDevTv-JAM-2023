public interface IPoolable
{
    /// <summary>
    /// The Pooling Manager from which this poolable will be instantiated and managed
    /// </summary>
    public PoolingManager PoolingManagerSO { get; set; }
}
