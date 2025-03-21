using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Repository.iUnitOfWork;
using DataAccess.Repository.IGenericRepository;
using Models;
using DataAccess.DbContext;
using Models.Model;

namespace DataAccess.Repository;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly ApplicationDbContext _db;
    private IGenericRepository<ApplicationUser>? _applicationUserRepository;
    public UnitOfWork(ApplicationDbContext db)
    {
        _db = db;
    }

    // In elke controller waar je de repository wilt gebruiken, moet je de UnitOfWork injecteren.
    public IGenericRepository<ApplicationUser> ApplicationUserRepository
    {
        get
        {
            _applicationUserRepository ??= new GenericRepository<ApplicationUser>(_db);
            return _applicationUserRepository;
        }
    }





    public void Dispose() => _db.Dispose();

    public async Task SaveAsync()
    {
       await _db.SaveChangesAsync();
    }
}
