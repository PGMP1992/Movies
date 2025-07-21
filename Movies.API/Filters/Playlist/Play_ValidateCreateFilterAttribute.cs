using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Movies.DataAccess.Data;

namespace Movies.API.Filters.Movie
{
    public class Play_ValidateCreateFilterAttribute : ActionFilterAttribute
    {
        private readonly ApplicationDbContext db;

        public Play_ValidateCreateFilterAttribute(ApplicationDbContext db)
        {
            this.db = db;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var playlist = context.ActionArguments["playlist"] as Movies.DataAccess.Models.Playlist;

            if (playlist == null)
            {
                context.ModelState.AddModelError("Playlist", "Playlist object is null.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
            }
            else
            {
                var existingPlay = db.Playlists.FirstOrDefault(x =>
                    !string.IsNullOrWhiteSpace(playlist.Name) &&
                    !string.IsNullOrWhiteSpace(x.Name) &&
                    x.Name.ToLower() == playlist.Name.ToLower());

                if (existingPlay != null)
                {
                    context.ModelState.AddModelError("Playlist", "Playlist already exists.");
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
