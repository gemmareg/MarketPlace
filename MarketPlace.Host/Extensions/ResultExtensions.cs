using MarketPlace.Shared.Result.Generic;
using MarketPlace.Shared.Result.NonGeneric;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Host.Extensions
{
    public static class ResultExtensions
    {
        public static ActionResult ToActionResult<T>(this Result<T> result)
        {
            if (result.Success) return new OkObjectResult(result.Data);

            return new BadRequestObjectResult(result.Message) { };
        }

        public static ActionResult ToActionResult(this Result result)
        {
            if (result.Success) return new OkObjectResult(result);

            return new BadRequestObjectResult(result.Message) { };
        }
    }
}
