using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApp_DataAccess.Data;
using WebApp_DataAccess.Repository.IRepository;
using WebAppMVC_Models;

namespace WebApp_DataAccess.Repository
{
    public class MyDailyJournalRepository : Repository<MyDailyJournalModel>, IMyDailyJournalRepository
    {
        public int forTestTransientOrScooped = 0;
        public MyDailyJournalRepository(ApplicationDbContext _db) : base(_db)
        {
        }


        public void Add(MyDailyJournalModel model, ClaimsPrincipal user)
        {
            model.CreatedDateTime = DateTime.Now;
            model.ModifiedDateTime = DateTime.Now;
            db.MyDailyJournal.Add(model);                
            
            db.SaveChanges();

            LogTableModel logTable = new LogTableModel();
            logTable.CreatedDateTime = DateTime.Now;
            logTable.TypeStr = "insert";
            logTable.LogTableName = model.GetType().Name;
            logTable.LogRecordId = model.Id;
            logTable.Message = model.Text;
            logTable.CreatedByUserId = user.Claims.FirstOrDefault().Value;
            db.LogTable.Add(logTable);             
        }
        public void Update(MyDailyJournalModel model, ClaimsPrincipal user)
        {
            var objPrev = this.FirstOrDefault( _ => _.Id == model.Id, isTracking: false);
            model.CreatedDateTime = objPrev.CreatedDateTime;
            model.ModifiedDateTime = DateTime.Now;
            db.MyDailyJournal.Update(model);

            LogTableModel logTable = new LogTableModel();
            logTable.CreatedDateTime = DateTime.Now;
            logTable.TypeStr = "modify";
            logTable.LogTableName = model.GetType().Name;
            logTable.LogRecordId = model.Id;
            logTable.Message = model.Text;
            logTable.CreatedByUserId = user.Claims.FirstOrDefault().Value;
            db.LogTable.Add(logTable);

            db.MyDailyJournal.Update(model);
        }

        public void Remove(MyDailyJournalModel model, ClaimsPrincipal user)
        { 
            db.MyDailyJournal.Remove(model);

            LogTableModel logTable = new LogTableModel();
            logTable.CreatedDateTime = DateTime.Now;
            logTable.TypeStr = "delete";
            logTable.LogTableName = model.GetType().Name;;
            logTable.Message = model.Text;
            logTable.LogRecordId = model.Id;
            logTable.CreatedByUserId = user.Claims.FirstOrDefault().Value;
            db.LogTable.Add(logTable);
        }






    }
}
