using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EFCoreExample.CustomModelBinders
{
    public class DateOnlyYyyyMmDdBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var val = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).FirstValue;
            if (string.IsNullOrWhiteSpace(val))
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }

            if(DateOnly.TryParse(val, out var date))
            {
                bindingContext.Result = ModelBindingResult.Success(date);
                return Task.CompletedTask;
            }

            bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Invalid date format. Use yyyyMMdd.");
            bindingContext.Result = ModelBindingResult.Failed();
            return Task.CompletedTask;  
        }
    }
}
