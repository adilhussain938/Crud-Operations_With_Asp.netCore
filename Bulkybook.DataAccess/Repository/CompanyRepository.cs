using Bulkybook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Bulkybook.DataAccess.Repository
{
    public class CompanyRepository : Repository<CompanyModel>, ICompanyRepository
    {
        private readonly DBContextsol _db;
        public CompanyRepository(DBContextsol db) : base(db)
        {
            _db = db;
        }
        public void Update(CompanyModel obj)
        {
            _db.Company.Update(obj);
        }
    }
}
