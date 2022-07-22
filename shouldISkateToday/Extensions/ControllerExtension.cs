using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace shouldISkateToday.Extensions;

public static class ControllerExtension
{
    public static IActionResult ToOk<TResult, TContract>(this Result<TResult> result, Func<TResult, TContract> contract)
        where TContract : class
    {
        return result.Match<IActionResult>(obj =>
        {
            var response = contract(obj);
            return new OkObjectResult(response);
        }, exception =>
        {
            return exception switch
            {
                ApiException => new StatusCodeResult(403),
                BadHttpRequestException => new StatusCodeResult(422),
                _ => new StatusCodeResult(500)
            };
        });
    }
}