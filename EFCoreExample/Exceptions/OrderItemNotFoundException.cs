namespace EFCoreExample.Exceptions
{
    public class OrderItemNotFoundException(string details): EFCoreExampleException(details)
    {
        public override ExceptionCode Code
            => ExceptionCode.ORDER_ITEM_NOT_FOUND;

        public override string Message 
            => "Item cold not be found in the order ";
    }
}
