using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Movies.DataAccess.Data;

namespace Movies.API.Filters.Movie
{
    public class Movie_ValidateCreateFilterAttribute : ActionFilterAttribute
    {
        private readonly ApplicationDbContext db;

        public Movie_ValidateCreateFilterAttribute(ApplicationDbContext db)
        {
            this.db = db;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var movie = context.ActionArguments["movie"] as Movies.DataAccess.Models.Movie;

            if (movie == null)
            {
                context.ModelState.AddModelError("Movie", "Movie does not exist.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
            }
            else
            {
                var existingMovie = db.Movies.FirstOrDefault(x =>
                    !string.IsNullOrWhiteSpace(movie.Title) &&
                    !string.IsNullOrWhiteSpace(x.Title) &&
                    x.Title.ToLower() == movie.Title.ToLower());

                if (existingMovie != null)
                {
                    context.ModelState.AddModelError("Movie", "Movie already exists.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDetails);
                }
            }

        }
    }
}
