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
    public class ChatRepository1 : Repository<ChatModel>, IChatRepository
    {
        public ChatRepository1(ApplicationDbContext _db) : base(_db)
        {
        }

        public void Add(ChatModel model)
        { 

            model.MessageDate = DateTime.Now;
            db.Chat.Add(model);                
         
            LogTableModel logTable = new LogTableModel();
            logTable.CreatedDateTime = DateTime.Now;
            logTable.TypeStr = "insert";
            logTable.LogTableName = "ChatHub";
            logTable.LogRecordId = 0;
            logTable.Message = model.MessageText;
            db.LogTable.Add(logTable);             

        }



    }
}
