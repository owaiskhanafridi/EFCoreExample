namespace EFCoreExample.Exceptions
{
    public class NegativePriceException(string details): EFCoreExampleException(details)

    {
        public override ExceptionCode Code
            => ExceptionCode.NEGATIVE_PRICE_EXCEPTION;

        public override string Message
            => "Negative Numbers are not acceptable for price";
    }
}
