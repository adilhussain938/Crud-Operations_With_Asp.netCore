using Bulkybook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bulkybook.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T: class
    {


        private readonly DBContextsol _db;
        internal DbSet<T> dbset;

        public Repository(DBContextsol db)
        {
            _db = db;
            this.dbset = _db.Set<T>();
        }

        IEnumerable<T> IRepository<T>.GetAll(Expression<Func<T, bool>> filter =null )
        {
            IQueryable<T> query = dbset;
            if (filter !=null)
            { 
            query = query.Where(filter);
            }
            return query.ToList();
        }   

        T IRepository<T>.GetFirstOrDefault(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbset;
            query  = query.Where(filter);
            return query.FirstOrDefault();
        }

        void IRepository<T>.Remove(T entity)
        {
            dbset.Remove(entity);
        }

        void IRepository<T>.RemoveRange(IEnumerable<T> entity)
        {
            dbset.RemoveRange(entity);
        }

        void IRepository<T>.Add(T entity)
        {
            dbset.Add(entity);
        }
    }

     
}
