using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulkybook.DataAccess.Repository.IRepository
{
    public  interface ICategoryRespository : IRepository<Category>
    {

        void Update(Category obj);
        //void Save();


         
    }
}
