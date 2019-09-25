using System;
using System.Collections.Generic;
using Newtonsoft.Json.Encryption;

public class MessageWithSecretData
{
    [Encrypt]
    public string? Secret { get; set; }
    public MySecretSubProperty? SubProperty { get; set; }
    public List<CreditCardDetails>? CreditCards { get; set; }
}

public class MySecretSubProperty
{
    [Encrypt]
    public string? Secret { get; set; }
}

public class CreditCardDetails
{
    public DateTime ValidTo { get; set; }
    [Encrypt]
    public string? Number { get; set; }
}