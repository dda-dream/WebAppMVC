using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp_DataAccess.Data;
using WebApp_DataAccess.Repository.IRepository;
using WebAppMVC_Models;

namespace WebApp_DataAccess.Repository
{
    public class MyDailyJournalRepository : Repository<MyDailyJournalModel>, IMyDailyJournalRepository
    {
        
        public MyDailyJournalRepository(ApplicationDbContext _db) : base(_db)
        {
        }

        public void Add(MyDailyJournalModel model)
        {
            //var transactionId = db.Database.BeginTransaction();

            model.CreatedDateTime = DateTime.Now;
            model.ModifiedDateTime = DateTime.Now;
            var newRecord = db.Add(model);                
            //db.SaveChanges();

            LogTableModel logTable = new LogTableModel();
            logTable.CreatedDateTime = DateTime.Now;
            logTable.TypeStr = "insert";
            logTable.LogTableName = "MyDailyJournalModel";
            logTable.LogRecordId = newRecord.Entity.Id;
            logTable.Message = $"MyDailyJournalRepository.cs Record with NO ID.  Text:{newRecord.Entity.Text}";
            db.Add(logTable);
            //db.SaveChanges();

            //transactionId.Commit();
        }
        public void Update(MyDailyJournalModel model)
        {

            var objPrev = this.FirstOrDefault( _ => _.Id == model.Id, isTracking: false);
            model.CreatedDateTime = objPrev.CreatedDateTime;
            model.ModifiedDateTime = DateTime.Now;
            db.Update(model);
            //db.SaveChanges();

            LogTableModel logTable = new LogTableModel();
            logTable.CreatedDateTime = DateTime.Now;
            logTable.TypeStr = "modify";
            logTable.LogTableName = "MyDailyJournalModel";
            logTable.LogRecordId = model.Id;
            logTable.Message = $"Record with ID:{model.Id}  Text:{model.Text}";
            db.Add(logTable);

            //db.SaveChanges();


            db.Update(model);


        }

        public void Remove(MyDailyJournalModel model)
        { 
            db.Remove(model);
            db.SaveChanges();

            LogTableModel logTable = new LogTableModel();
            logTable.CreatedDateTime = DateTime.Now;
            logTable.TypeStr = "delete";
            logTable.LogTableName = "MyDailyJournalModel";
            logTable.Message = $"Record with ID:{model.Id}  Text:{model.Text}";
            logTable.LogRecordId = model.Id;
            db.Add(logTable);
            db.SaveChanges();
        }
    }
}
