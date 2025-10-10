using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WebApp_Utility
{
    public enum MyDailyJournal_ExecutionStatus 
    {
        [Description("Создано")]
        Created = 10, 
        [Description("В процессе")]
        InProgress = 20, 
        [Description("Завершено")]
        Finished = 30, 
        [Description("Отменено")]
        Cancelled = 40 
    }

    
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            if (value == null) return string.Empty;

            var field = value.GetType().GetField(value.ToString());
            var attribute = field?.GetCustomAttribute<DescriptionAttribute>();
            return attribute?.Description ?? value.ToString();
        }
    }

}
