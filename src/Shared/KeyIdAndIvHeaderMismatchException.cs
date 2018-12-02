using System;

namespace Newtonsoft.Json.Encryption
{
    class KeyIdAndIvHeaderMismatchException : Exception
    {
        public KeyIdAndIvHeaderMismatchException() :
            base($"Either the header values for {HeaderExtractor.KeyId} and {HeaderExtractor.Iv} must both be empty, or both be non empty.")
        {
        }
    }
}