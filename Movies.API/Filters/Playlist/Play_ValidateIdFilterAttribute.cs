using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Movies.DataAccess.Data;

namespace Movies.API.Filters.Movie
{
    public class Play_ValidateIdFilterAttribute : ActionFilterAttribute
    {
        private ApplicationDbContext _db;

        public Play_ValidateIdFilterAttribute(ApplicationDbContext db)
        {
            _db = db;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var Id = context.ActionArguments["id"] as int?;

            if (Id.HasValue)
            {
                if (Id.Value <= 0)
                {
                    context.ModelState.AddModelError("Id", "Id is invalid");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDetails);
                }
                else
                {
                    var play = _db.Playlists.Find(Id.Value);

                    if (play == null)
                    {
                        context.ModelState.AddModelError("Id", "Playlist doesn't exist.");
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status404NotFound
                        };
                        context.Result = new NotFoundObjectResult(problemDetails);
                    }
                    else
                    {
                        context.HttpContext.Items["playlist"] = play;
                    }
                }
            }
        }

    }
}
