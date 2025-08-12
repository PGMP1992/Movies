using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Movies.DataAccess.Data;

namespace Movies.API.Filters.Movie
{
    public class Movie_ValidateSearchFilterAttribute : ActionFilterAttribute
    {
        private ApplicationDbContext _db;

        public Movie_ValidateSearchFilterAttribute(ApplicationDbContext db)
        {
            _db = db;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var search = context.ActionArguments["search"] as string;

            if (!string.IsNullOrEmpty(search))
            {
                var movies = _db.Movies.FirstOrDefault(x => x.Title.Contains(search));

                if (movies == null)
                {
                    context.ModelState.AddModelError("movies", "There are no movies with that name.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status404NotFound
                    };
                    context.Result = new NotFoundObjectResult(problemDetails);
                }
                else
                {
                    context.HttpContext.Items["movies"] = movies;
                }
            }
            else
            {
                context.ModelState.AddModelError("search", "Please enter a valid search");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
            }
        }
    }
}
