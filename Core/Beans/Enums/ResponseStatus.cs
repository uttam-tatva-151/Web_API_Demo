using System.Runtime.Serialization;

namespace Core.Beans.Enums
{
    public enum ResponseStatus
    {
        [EnumMember(Value = "Success")]
        Success,     // Operation completed successfully
        [EnumMember(Value = "Error")]
        Error,      // Operation failed
        [EnumMember(Value = "Warning")]
        Warning,     // Operation completed with a warning
        [EnumMember(Value = "NotFound")]
        NotFound,    // Resource not found
        [EnumMember(Value = "Unauthorized")]
        Unauthorized,// Operation not authorized
        [EnumMember(Value = "ValidationError")]
        ValidationError // Input validation failed
    }  
}
