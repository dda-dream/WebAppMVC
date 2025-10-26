using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebAppMVC_Models;

namespace WebApp_DataAccess.Repository.IRepository
{
    public interface IMyDailyJournalRepository : IRepository<MyDailyJournalModel>
    {
        void Add(MyDailyJournalModel model, ClaimsPrincipal user);
        void Update(MyDailyJournalModel model, ClaimsPrincipal user);
        void Remove(MyDailyJournalModel model, ClaimsPrincipal user);
    }
}
