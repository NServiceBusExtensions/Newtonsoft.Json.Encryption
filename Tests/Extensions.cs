using System.IO;
using System.Text;
using Newtonsoft.Json;

public static class Extensions
{
    public static T Deserialize<T>(this JsonSerializer serializer, string value)
    {
        using (var reader = new StringReader(value))
        using (var jsonReader = new JsonTextReader(reader))
        {
            return serializer.Deserialize<T>(jsonReader);
        }
    }

    public static string Serialize<T>(this JsonSerializer serializer, T target)
    {
        var builder = new StringBuilder();
        using (var writer = new StringWriter(builder))
        {
            serializer.Serialize(writer, target);
        }

        return builder.ToString();
    }
}