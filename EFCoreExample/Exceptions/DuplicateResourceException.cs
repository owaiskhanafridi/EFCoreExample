namespace EFCoreExample.Exceptions
{
    public class DuplicateResourceException(string details): EFCoreExampleException(details)
    {
        public override ExceptionCode Code
            => ExceptionCode.DUPLICATE_RESOURCE_EXCEPTION;

        public override string Message 
            => "Duplication Occurs";
    }
}
