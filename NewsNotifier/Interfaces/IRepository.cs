namespace NewsNotifier.Interfaces
{
    public interface IRepository
    {
        public interface IRepository<T>
        {
            void Save(T entity);
            void Delete(int id);
            T? FindById(int id);
            List<T> FindAll();
        }
    }
}
