using Core.Entities;
using Data.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.UnitOfWork.Abstract
{
    public interface IUnitOfWork
    {
        IRepository<Group> Groups { get; }
        IRepository<Student> Students { get; }
        void SaveChanges();
        void Commit();
    }
}
