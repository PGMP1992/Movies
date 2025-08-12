using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Movies.DataAccess.Data;

namespace Movies.API.Filters.Movie
{
    public class Movie_ValidateIdFilterAttribute : ActionFilterAttribute
    {
        private ApplicationDbContext _db;

        public Movie_ValidateIdFilterAttribute(ApplicationDbContext db)
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
                    var movie = _db.Movies.Find(Id.Value);

                    if (movie == null)
                    {
                        context.ModelState.AddModelError("Id", "Movie does not exist.");
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status404NotFound
                        };
                        context.Result = new NotFoundObjectResult(problemDetails);
                    }
                    else
                    {
                        context.HttpContext.Items["movie"] = movie;
                    }
                }
            }
        }

    }
}
