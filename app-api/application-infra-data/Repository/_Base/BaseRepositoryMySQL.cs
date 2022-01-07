using Microsoft.EntityFrameworkCore.ChangeTracking;
using application_domain.Abstracts;
using application_infra_data.Context;
using System;

namespace application_infra_data.Repository
{
    public class BaseRepositoryMySQL<TEntity, TKeyType> where TEntity : BaseEntity<TKeyType>
    {
        protected readonly MySqlContext _context;

        public BaseRepositoryMySQL(MySqlContext mySqlContext)
        {
            _context = mySqlContext;
        }

        protected virtual void Insert(TEntity obj)
        {
            _context.Set<TEntity>().Add(obj);
            _context.SaveChanges(); // nunca save changes para cada request

            // se voce precisar usar um serviço que use 3 repositorios, serao 3 requests no banco devido o save changes
            // para isso vc usa o pattern unit of work e o save changes fica la um "unico commit" para toda a transaçao
        }

        protected virtual void Update(TEntity obj)
        {
            _context.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges(); // nunca save changes para cada request
        }

        protected virtual void Update<TProperty>(
            TEntity obj,
            params PropertyEntry<TEntity, TProperty>[] propsToIgnore)
        {
            _context.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            foreach (var item in propsToIgnore)
                item.IsModified = false;

            _context.SaveChanges(); // nunca save changes para cada request
        }

        protected virtual void Delete(int id)
        {
            var objEntity = Select(id);

            if (objEntity != null)
            {
                _context.Set<TEntity>().Remove(objEntity);
                _context.SaveChanges();
            }
        }

        protected virtual IList<TEntity> Select() =>
            _context.Set<TEntity>().ToList();

        protected virtual TEntity? Select(int id) =>
            _context.Set<TEntity>().Find(id);
    }
}