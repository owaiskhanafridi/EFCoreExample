using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace EFCoreExample.Interceptors
{
    public class SqlCommandLoggerInterceptor : DbCommandInterceptor
    {
        public override InterceptionResult<DbDataReader> ReaderExecuting(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<DbDataReader> result)
        {
            Console.WriteLine($"Logged Query by Interceptor: {command.CommandText}");
            return base.ReaderExecuting(command, eventData, result);
        }
    }
}
