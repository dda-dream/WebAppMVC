using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
    public class LogTableRepository : Repository<LogTableModel>, ILogTableRepository
    {
        public LogTableRepository(ApplicationDbContext _db) : base(_db)
        {
        }

        public void Add(LogTableModel model)
        { 
            //db.LogTable.Include(c => c.Id).ThenInclude
            /*         
            LogTableModel logTable = new LogTableModel();
            logTable.CreatedDateTime = DateTime.Now;
            logTable.TypeStr = "insert";
            logTable.LogTableName = "ChatHub";
            logTable.LogRecordId = 0;
            logTable.Message = model.MessageText;
            */
            db.LogTable.Add(model);             

        }



    }
}
