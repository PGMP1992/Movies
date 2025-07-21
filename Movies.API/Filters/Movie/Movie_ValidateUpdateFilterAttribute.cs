using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MoviesAPI.Filters.Movie
{
    public class Movie_ValidateUpdateFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var id = context.ActionArguments["id"] as int?;
            var playlist = context.ActionArguments["playlist"] as Movies.DataAccess.Models.Playlist;

            if (id.HasValue && playlist != null && id != playlist.Id)
            {
                context.ModelState.AddModelError("Id", "Id is not the same as id.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
            }
        }
    }
}