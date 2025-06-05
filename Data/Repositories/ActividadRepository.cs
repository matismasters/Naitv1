
namespace Naitv1.Data.Repositories
{
    public class ActividadRepository : IActividadRepository
    {
        private readonly AppDbContext _appDbContext;

        public ActividadRepository(AppDbContext context)
        {
            _appDbContext = context;
        }

        public int ContarActividades()
        {
            return _appDbContext.Actividades.Count();
        }
    }
}
