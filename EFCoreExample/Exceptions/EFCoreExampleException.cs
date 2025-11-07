namespace EFCoreExample.Exceptions
{
    public abstract class EFCoreExampleException(string details): Exception(details)
    {
        public abstract ExceptionCode Code { get; }
        public abstract override string Message { get; }
        
        public readonly string Details = details;
    }
}
