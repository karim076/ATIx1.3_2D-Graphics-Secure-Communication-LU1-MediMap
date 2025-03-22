using DataAccess.Repository.IGenericRepository;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.iUnitOfWork
{
    public interface IUnitOfWork
    {
        IGenericRepository<ApplicationUser> ApplicationUserRepository { get; }
        Task SaveAsync();
    }
}
