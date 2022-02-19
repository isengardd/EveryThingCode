using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class JsonTool
{
    public static string ToJson(object obj, Formatting formatting = Formatting.None)
    {
        return JToken.FromObject(obj).ToString(formatting);
    }

    public static T ToObject<T>(string json)
    {
        return JToken.Parse(json).ToObject<T>();
    }
}
