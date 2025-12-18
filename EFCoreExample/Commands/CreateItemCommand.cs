using EFCoreExample.CustomModelBinders;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreExample.Commands
{
    public record CreateItemCommand (string title, decimal price, DateTime createdAt);
    
}
