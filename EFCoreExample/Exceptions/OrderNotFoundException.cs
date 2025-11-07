namespace EFCoreExample.Exceptions
{
    public class OrderNotFoundException(string details): EFCoreExampleException(details)
    {
        public override ExceptionCode Code
            => ExceptionCode.ORDER_ITEM_NOT_FOUND;

        public override string Message 
            => "Order Not Found";
    }
}
