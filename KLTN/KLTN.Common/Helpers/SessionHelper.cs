using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace KLTN.Common.Helpers
{
    public static class SessionHelper
    {
        public static void SetObjectAsJson(ISession session, string key, object value)
        {
            var seriValue = JsonConvert.SerializeObject(value);
            session.SetString(key, seriValue);
        }

        public static T GetObjectFromJson<T>(ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
