using Bulkybook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulkybook.DataAccess.Repository
{
    public class ShopingCartRepository : Repository<ShoppingCart>, IShopingCartRepository
    {
        private readonly DBContextsol _db;
        public ShopingCartRepository(DBContextsol db): base(db)
        {
            _db = db;
        }

        public int decrementshoppingcart(ShoppingCart shopingcart, int count)
        {
            shopingcart.count -= count;
            return shopingcart.count;
        }

        public int incrementshoppingcart(ShoppingCart shopingcart, int count)
        {

            shopingcart.count += count;
            return shopingcart.count;
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
