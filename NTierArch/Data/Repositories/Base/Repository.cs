using Core.Entities;
using Core.Entities.Base;
using Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Base
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private AppDbContext _context;
        private readonly DbSet<T> _dbTable;
        public Repository(AppDbContext context)
        {
            _context = context;
            _dbTable = _context.Set<T>();
        }
        public List<T> GetAll()
        {
            return _dbTable.ToList();
        }
        public T Get(int id)
        {
            return _dbTable.Find(id);
        }
        public void Add(T item)
        {
            _dbTable.Add(item);
        }
        public void Update(T item)
        {
            _dbTable.Update(item);
        }

        public void Delete(T item)
        {
            _dbTable.Remove(item);
        }
        public void Commit()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }


        }


    }
}
