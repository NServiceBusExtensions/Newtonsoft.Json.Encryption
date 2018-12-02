using System;

namespace Newtonsoft.Json.Encryption
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class EncryptAttribute : Attribute
    {
    }
}