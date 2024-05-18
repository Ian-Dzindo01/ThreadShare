namespace ThreadShare.Repository.Interfaces
{
    public interface IRepository<TEntity> where TEntity: class
    {
        Task<TEntity> GetById(int id);
        Task Add(TEntity entity);
        Task Delete(int id);
        Task Update(TEntity entity);
        Task<bool> InstanceExists(int id);
        //Task<List<TEntity>> GetAll();
    }
}
