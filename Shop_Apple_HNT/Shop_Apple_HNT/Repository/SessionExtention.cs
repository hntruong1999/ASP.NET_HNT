using Newtonsoft.Json;
using Shop_Apple_HNT.Controllers;
using Shop_Apple_HNT.Models;
using System.Runtime.CompilerServices;

namespace Shop_Apple_HNT.Repository
{
    public static class SessionExtention
    {
        public static void SetJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
           
        }
        public static T GetJson<T>(this ISession session, string key)
        {
            var sessionData = session.GetString(key);
            return sessionData == null ? default(T) : JsonConvert.DeserializeObject<T>(sessionData);
        }
       
    }
}
