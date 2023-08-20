using BulkyBook.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulkybook.DataAccess.Repository.IRepository
{
    public interface IUnitOfwork
    {
        ICategoryRespository Category { get; }

        ICoverTypeRepository CoverType { get; }
        IProductTypeRepository Product { get; }
        
        IShopingCartRepository ShopingCartRepository { get; }
        
        ICompanyRepository Company { get; }

        IOrderHeaderRepository OrderHeader { get; }
		IApplicationUserRepository ApplicationUserRepository { get; }

        IOrderDetailRepository OrderDetail { get; }
        void save();
    }
}
 