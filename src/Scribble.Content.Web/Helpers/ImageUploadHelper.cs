namespace Scribble.Content.Web.Helpers;

public static class ImageUploadHelper
{
    public static string CreateUniqueName(string fileName)
    {
        return string.Concat(Path.GetFileNameWithoutExtension(fileName),
            "_", Guid.NewGuid().ToString().AsSpan(0, 4),
            Path.GetExtension(fileName));
    }
}