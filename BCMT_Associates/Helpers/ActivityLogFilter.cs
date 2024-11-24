using BCMT_Associates.Interfaces;
using BCMT_Associates.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace BCMT_Associates.Helpers
{
    public class ActivityLogFilter : IActionFilter
    {
        private readonly IActivityLogService _activityLogService;

        public ActivityLogFilter(IActivityLogService activityLogService)
        {
            _activityLogService = activityLogService;
        }

        public async void OnActionExecuting(ActionExecutingContext context)
        {
            var actionName = context.ActionDescriptor.DisplayName;
            if (actionName.Contains("Get", StringComparison.OrdinalIgnoreCase) ||
                actionName.Contains("Login", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }
            else if (context.Result is ViewResult)
            {
                return;
            }
            var userId = context.HttpContext.User.Identity.Name;
            var action = context.ActionDescriptor.DisplayName;
            var actionArguments = context.ActionArguments;

            // Serialize the action arguments to JSON
            var actionArgumentsJson = JsonSerializer.Serialize(actionArguments, new JsonSerializerOptions
            {
                WriteIndented = true
            });


            if(actionArguments.Count > 0)
            {
                await _activityLogService.LogAsync(new ActivityLog
                {
                    UserId = userId,
                    Action = action,
                    Description = actionArgumentsJson,
                    Timestamp = DateTime.UtcNow
                });
            }

           
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Do nothing
        }
    }

}
