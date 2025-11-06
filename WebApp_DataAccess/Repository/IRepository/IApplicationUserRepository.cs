using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp_Models;
using WebAppMVC_Models;

namespace WebApp_DataAccess.Repository.IRepository
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
        void Update(ApplicationUser applicationUser);


        public IEnumerable<ApplicationUser> GetUsersForLog(int logRecordId, string logTableName);

        public int GetUserChatMessagesCount(string userid);
        public Dictionary<DateTime, int> GetUserChatMessagesCountByDay(string userid);

    }
}
