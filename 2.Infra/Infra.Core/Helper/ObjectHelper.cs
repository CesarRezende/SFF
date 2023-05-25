using Newtonsoft.Json;

namespace SFF.Infra.Core.Helper
{
    public static class ObjectHelper
    {

        public static string ToJsonFormat(this object ObjectToTranform)
        {
            return JsonConvert.SerializeObject(ObjectToTranform, Formatting.Indented, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });
        }
    }
}
