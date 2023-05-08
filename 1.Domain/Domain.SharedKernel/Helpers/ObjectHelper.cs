using Newtonsoft.Json;

namespace SFF.SharedKernel.Helpers
{
    public static class ObjectHelper
    {

        public static string ToJsonFormat(this object ObjectToTranform)
        {
            return JsonConvert.SerializeObject(ObjectToTranform, Formatting.Indented);
        }
    }
}
