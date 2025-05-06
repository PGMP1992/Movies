namespace MoviesApp.DTOs
{
    public record MovieDTO(
         int Id
        ,string Title
        ,string Description
        ,string Genre
        ,int Age
        ,bool Active
        ,string? PictUrl
        ,DateTimeOffset Created,
        DateTimeOffset LastModified);
}
