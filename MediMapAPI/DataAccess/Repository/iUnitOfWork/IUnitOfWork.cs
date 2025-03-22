using DataAccess.Repository.IGenericRepository;
using Models;
using Models.Model;
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
        IGenericRepository<Arts> ArtsRepository { get; }
        IGenericRepository<Traject> TrajectRepository {  get; }
        IGenericRepository<Patient> PatientRepository { get; }
        IGenericRepository<OuderVoogd> OuderVoogdRepository { get; }
        IGenericRepository<ZorgMoment> ZorgMomentRepository { get; }
        IGenericRepository<Avatar> AvatarRepository { get; }
        IGenericRepository<LogBook> LogBookRepository { get; }
        IGenericRepository<ProfileInformation> ProfileInformationRepository { get; }
        Task SaveAsync();
    }
}
