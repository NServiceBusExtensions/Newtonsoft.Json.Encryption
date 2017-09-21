using Newtonsoft.Json.Encryption;

public class ClassToSerialize
{
    [Encrypt]
    public string Property1 { get; set; }

    [Encrypt]
    public string Property2 { get; set; }
}