using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebAppMVC_Models.ViewModels
{
    public class MyDailyJournal_VM
    {
        public MyDailyJournalModel MyDailyJournal { get; set; }

        public IEnumerable<LogTableModel> LogTableEnumerator { get; set; }

    }
}
