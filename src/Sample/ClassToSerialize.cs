using Newtonsoft.Json.Encryption;

public class ClassToSerialize
{
    [Encrypt]
    public string Property { get; set; }
}