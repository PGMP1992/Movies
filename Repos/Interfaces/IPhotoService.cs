using CloudinaryDotNet.Actions;

namespace MoviesApp.Repos.Interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
        Task<DeletionResult> DeletePhotoAsync(string publicId);


    }
}
