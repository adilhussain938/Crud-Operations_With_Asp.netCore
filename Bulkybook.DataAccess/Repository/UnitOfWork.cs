using Bulkybook.DataAccess.Repository.IRepository;
using BulkyBook.DataAccess.Repository;
using BulkyBook.DataAccess.Repository.IRepository;
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
            ShopingCartRepository = new ShopingCartRepository(_db);
            OrderHeader = new OrderHeaderRepository(_db);
            OrderDetail = new OrderDetailRepository(_db);
            ApplicationUserRepository = new ApplicationUserRepository(_db);


		}


        public ICategoryRespository Category { get; private set; }
        public ICoverTypeRepository CoverType { get; private set; }
        public IProductTypeRepository Product { get; private set; }
        public ICompanyRepository Company { get; private set; }
        public IShopingCartRepository ShopingCartRepository { get; private set; }

        public IOrderHeaderRepository OrderHeader { get; private set; }
        public IOrderDetailRepository OrderDetail { get; private set; }
        public IApplicationUserRepository ApplicationUserRepository { get; private set; }


        public void save()
        { 
            _db.SaveChanges();
        }
    }
}
