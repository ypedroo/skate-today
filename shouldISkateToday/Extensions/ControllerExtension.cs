using LanguageExt.Common;
using LanguageExt.TypeClasses;
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
    public static IActionResult ToCreated<TResult>(this Result<TResult> result, Func<TResult, bool> contract)
    
    {
        return result.Match<IActionResult>(obj =>
        {
            var response = contract(obj);
            return new CreatedResult("Created",response);
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