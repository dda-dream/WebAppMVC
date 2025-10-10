using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp_DataAccess.Data;
using WebApp_DataAccess.Repository.IRepository;
using WebApp_Models;
using WebAppMVC_Models;

namespace WebApp_DataAccess.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        
        public ApplicationUserRepository(ApplicationDbContext _db) : base(_db)
        {
        }

        public void Update(ApplicationUser applicationUser)
        {
            db.ApplicationUser.Update(applicationUser);
        }
    }
}
