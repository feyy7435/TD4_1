using Microsoft.AspNetCore.Mvc;

namespace TD4.Models.Repository
{
    public interface IDataRepository<TEntity>
    {
        ActionResult<IEnumerable<TEntity>> GetAll();
        ActionResult<TEntity> GetById(int id);
        ActionResult<TEntity> GetByString(string str);
        void Add(TEntity entity);
        void Update(TEntity entityToUpdate, TEntity entity);
        void Delete(TEntity entity);
    }
}
