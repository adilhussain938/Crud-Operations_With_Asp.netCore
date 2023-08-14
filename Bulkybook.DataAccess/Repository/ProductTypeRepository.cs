//using Bulkybook.DataAccess.Migrations;
using Bulkybook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulkybook.DataAccess.Repository
{
    public class ProductTypeRepository : Repository<BulkyBook.Models.Product>, IProductTypeRepository
    {
        private readonly DBContextsol _db;
        public ProductTypeRepository(DBContextsol db) : base(db)
        {
            _db = db;
        }

        public void Update(BulkyBook.Models.Product  obj)
        {
            var objfromdb = _db.Products.FirstOrDefault(u => u.Id == obj.Id);
            if(objfromdb != null)
            {
                objfromdb.Title = obj.Title;
                objfromdb.ISBN = obj.ISBN;
                objfromdb.Price = obj.Price;
                objfromdb.Price50 = obj.Price50;
                objfromdb.Price100 = obj.Price100;
                objfromdb.ListPrice = obj.ListPrice;
                objfromdb.Description = obj.Description;
                objfromdb.CategoryId = obj.CategoryId;
                objfromdb.Author = obj.Author;
                objfromdb.CoverTypeId = obj.CoverTypeId;
                objfromdb.Author = obj.Author;
                if(obj.ImageUrl!=null)
                {
                    objfromdb.ImageUrl = obj.ImageUrl;
                }
                _db.Entry(objfromdb).State = EntityState.Detached;
                _db.Products.Update(obj);
            }
        }
    }
}
