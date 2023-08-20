using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulkybook.DataAccess.Repository.IRepository
{
    public  interface IShopingCartRepository : IRepository<ShoppingCart>
    {

        int incrementshoppingcart(ShoppingCart shopingcart, int count);
        int decrementshoppingcart(ShoppingCart shopingcart, int count);


         
    }
}
