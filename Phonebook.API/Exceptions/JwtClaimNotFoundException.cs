using System;
using System.Runtime.Serialization;

namespace Phonebook.API.Exceptions
{
  [Serializable]
  public class JwtClaimNotFoundException : Exception
  {
    public JwtClaimNotFoundException()
    {
    }

    public JwtClaimNotFoundException(string message) : base(message)
    {
    }

    public JwtClaimNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected JwtClaimNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
  }
}