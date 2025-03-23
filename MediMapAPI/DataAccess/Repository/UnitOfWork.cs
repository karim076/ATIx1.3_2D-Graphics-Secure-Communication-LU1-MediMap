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
    private IGenericRepository<Arts>? _arts;
    private IGenericRepository<Traject>? _traject;
    private IGenericRepository<Patient>? _patient;
    private IGenericRepository<OuderVoogd>? _ouderVoogd;
    private IGenericRepository<ZorgMoment>? _zorgMoment;
    private IGenericRepository<Avatar>? _avatar;
    private IGenericRepository<LogBook>? _logBook;
    private IGenericRepository<ProfileInformation>? _profileInformation;
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
    public IGenericRepository<Arts> ArtsRepository
    {
        get
        {
            _arts ??= new GenericRepository<Arts>(_db);
            return _arts;
        }
    }
    public IGenericRepository<Traject> TrajectRepository
    {
        get
        {
            _traject ??= new GenericRepository<Traject>(_db);
            return _traject;
        }
    }
    public IGenericRepository<Patient> PatientRepository
    {
        get
        {
            _patient ??= new GenericRepository<Patient>(_db);
            return _patient;
        }
    }
    public IGenericRepository<OuderVoogd> OuderVoogdRepository
    {
        get
        {
            _ouderVoogd ??= new GenericRepository<OuderVoogd>(_db);
            return _ouderVoogd;
        }
    }
    public IGenericRepository<ZorgMoment> ZorgMomentRepository
    {
        get
        {
            _zorgMoment ??= new GenericRepository<ZorgMoment>(_db);
            return _zorgMoment;
        }
    }
    public IGenericRepository<Avatar> AvatarRepository
    {
        get
        {
            _avatar ??= new GenericRepository<Avatar>(_db);
            return _avatar;
        }
    }
    public IGenericRepository<LogBook> LogBookRepository
    {
        get
        {
            _logBook ??= new GenericRepository<LogBook>(_db);
            return _logBook;
        }
    }
    public IGenericRepository<ProfileInformation> ProfileInformationRepository
    {
        get
        {
            _profileInformation ??= new GenericRepository<ProfileInformation>(_db);
            return _profileInformation;
        }
    }
    public void Dispose() => _db.Dispose();

    public async Task SaveAsync()
    {
       await _db.SaveChangesAsync();
    }
}
