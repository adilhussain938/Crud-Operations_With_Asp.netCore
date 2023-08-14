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

        ICompanyRepository Company { get; }
        void save();
    }
}
 