//using Bulkybook.DataAccess.Migrations;
using Bulkybook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulkybook.DataAccess.Repository
{
    public class CoverTypeRepository: Repository<BulkyBook.Models.CoverType>,ICoverTypeRepository
    {
        private readonly DBContextsol _db;
        public CoverTypeRepository(DBContextsol db) : base(db)
        {
            _db = db;
        }

        public void Update(BulkyBook.Models.CoverType obj)
        {
            _db.CoverTypes.Update(obj);
        }
    }
}
