using System.Text.Json;

namespace WebAppMVC.Utility
{
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            var t = JsonSerializer.Serialize(value);
            session.SetString(key, t); 
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            if (value != null)
            {
                var d = JsonSerializer.Deserialize<T>(value);
                return d;
            }
            else
            {
                var d = default(T);
                return d;
            }
                //return value == null ? default : d;
        }
    }
}
  