using Core.Entities;
using Data.Contexts;
using Data.Repositories.Base;
using Data.Repositories.Concrete;
using Data.UnitOfWork.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.UnitOfWork.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        public readonly GroupRepository Groups;
        public readonly StudentRepository Students;
        private readonly AppDbContext _context;



        public UnitOfWork()
        {
            _context = new AppDbContext();
            Groups = new GroupRepository(_context);
            Students = new StudentRepository(_context);
        }

        public void SaveChanges()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                Console.WriteLine("Error occurred");
            }
        }

        public void Commit()
        {
            SaveChanges();
        }


    }
}
