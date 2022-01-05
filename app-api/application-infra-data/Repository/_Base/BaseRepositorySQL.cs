using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using application_domain.Abstracts;
using application_infra_data.Context;

namespace application_infra_data.Repository
{
    public class BaseRepositorySQL<TEntity, TKeyType> where TEntity : BaseEntity<TKeyType>
    {
        protected readonly SqlContext _context;

        public BaseRepositorySQL(SqlContext context)
        {
            _context = context;
        }

        protected virtual void Insert(TEntity obj)
        {
            _context.Set<TEntity>().Add(obj);

            _context.SaveChanges();
        }

        protected virtual void Update(TEntity obj)
        {
            _context.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            _context.SaveChanges();
        }

        protected virtual void Update<TProperty>(
            TEntity obj,
            params PropertyEntry<TEntity, TProperty>[] propsToIgnore)
        {
            _context.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            foreach (var item in propsToIgnore)
                item.IsModified = false;

            _context.SaveChanges();
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
