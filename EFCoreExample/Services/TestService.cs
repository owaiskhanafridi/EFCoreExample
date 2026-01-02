using EFCoreExample.Infrastructure;
using EFCoreExample.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCoreExample.Services
{
    /// <summary>
    /// This service just have test code
    /// </summary>
    public class TestService
    {
        private readonly AmazonDbContext _context;
        public TestService(AmazonDbContext dbContext)
        {
            _context = dbContext;
        }

        //Declaring a delegate
        delegate int myDelegate(int a, int b);

        delegate string StringTransformer(string s);
        int Add(int a, int b) => a + b;

        //Defining normal methods
        int Multiply(int a, int b) => a * b;

        string Capitalize(string s)
            => s.ToUpper();

        string Reverse(string s)
            => new string(s.Reverse().ToArray());

        Func<AmazonDbContext, decimal, IAsyncEnumerable<Item>> byPrice
            = EF.CompileAsyncQuery((AmazonDbContext db, decimal price)
                => db.Items.AsNoTracking().Where(x => x.Price > price));

        public IAsyncEnumerable<Item> TestFuncDelegate(decimal price)
        => byPrice(_context, price);
        

        public int DelegatePractice()
        {
            //Assigning a method to a delegate
            myDelegate op = Add;

            op = Multiply;

            var result = op(3, 5);  

            StringTransformer st = Capitalize;
            var resultString = st("Owais Khan");

            StringTransformer st2 = Reverse;

            var reverseResult = st2("University");


            Func<int, int, int> funcOperation = Add;
            Console.WriteLine($"Func Result is:  {funcOperation(2, 4).ToString()}");

            return result;
        }
        
        public string CreateDateWithSpecificTimeZone()
        {
            var tz = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
            var local = new DateTime(2025, 12, 22, 9, 0, 0, DateTimeKind.Unspecified);
            var dto = new DateTimeOffset(local, tz.GetUtcOffset(local));

            return dto.ToString();
        }


    }
}
