using Bulkybook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulkybook.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfwork
    {
        private DBContextsol _db;
        public UnitOfWork(DBContextsol dB)
        {
            _db = dB;
            Category = new CategoryRepository(_db);
            CoverType = new CoverTypeRepository(_db);
            Product = new ProductTypeRepository(_db);
            Company = new CompanyRepository(_db);
        }

        public ICategoryRespository Category { get; private set; }
        public ICoverTypeRepository CoverType { get; private set; }
        public IProductTypeRepository Product { get; private set; }
        public ICompanyRepository Company { get; private set; }


        public void save()
        { 
            _db.SaveChanges();
        }
    }
}
