using Bulkybook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulkybook.DataAccess.Repository
{
    public class CategoryRepository :Repository<Category>, ICategoryRespository
    {
        private readonly DBContextsol _db;
        public CategoryRepository(DBContextsol db): base(db)
        {
            _db = db;
        }

        //public void Save()
        //{
        //    _db.SaveChanges();
        //}
        
        public void Update(Category obj)
        {
            _db.Categories.Update(obj); 
        }
    }
}
