using Newtonsoft.Json;
using Xunit;

public class ReverseConverter :
    JsonConverter
{
    public static bool WriteCalled;
    public static bool ReadCalled;

    public static void AssertReadWriteCalled()
    {
        Assert.True(WriteCalled);
        Assert.True(ReadCalled);
    }

    public ReverseConverter()
    {
        WriteCalled = false;
        ReadCalled = false;
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        WriteCalled = true;
        if (value is string stringValue)
        {
            writer.WriteValue(stringValue.ReverseToString());
            return;
        }
        if (value is Guid guidValue)
        {
            writer.WriteValue(guidValue.ToString().ReverseToString());
            return;
        }
        if (value is byte[] byteValue)
        {
            writer.WriteValue(byteValue.Reverse().ToArray());
            return;
        }
    }

    public override object? ReadJson(JsonReader reader, Type type, object? existingValue, JsonSerializer serializer)
    {
        ReadCalled = true;
        if (type == typeof(string))
        {
            var stringValue = reader.ReadAsString();
            return stringValue!.ReverseToString();
        }
        if (type == typeof(Guid))
        {
            var stringValue = reader.ReadAsString();
            return new Guid(stringValue!.ReverseToString());
        }
        if (type == typeof(byte[]))
        {
            var bytes = reader.ReadAsBytes();
            return bytes.Reverse().ToArray();
        }
        return null;
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(string) ||
               objectType == typeof(byte[]) ||
               objectType == typeof(Guid);
    }
}