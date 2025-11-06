using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq.Expressions;
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




        public IEnumerable<ApplicationUser> GetUsersForLog(int logRecordId, string logTableName)
        {
            var result = db.ApplicationUser
                .Join(
                    db.LogTable.Where(t => t.LogRecordId == logRecordId && t.LogTableName == logTableName),
                    user => user.Id,
                    log => log.CreatedByUserId
                    ,
                    (user, log) => user
                )
                .OrderByDescending(s => s.FullName)
                .ToList();

            return result;
        }


        public int GetUserChatMessagesCount(string userid)
        {
            var count = db.Chat.Count(k => k.ApplicationUserId == userid);

            return count;
        }

        T f<TSource, TResult, T>(TSource a, int b, TResult c)
        {
            T t = default(T);

            return t;
        }

        public Dictionary<DateTime, int> GetUserChatMessagesCountByDay(string userid)
        {
            var r0 = db.Chat
                        .Where(i => i.ApplicationUserId == userid)
                        .GroupBy(i => i.MessageDate.Date)
                        .Select(i => new { d = i.Key, q = i.Count() }  )
                        ;

            var r1 = r0.ToDictionary(k=>k.d, k=>k.q);

            return r1;
        }



    }
}
