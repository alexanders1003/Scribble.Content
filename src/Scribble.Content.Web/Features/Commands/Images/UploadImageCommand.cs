using MediatR;
using Scribble.Content.Web.Helpers;

namespace Scribble.Content.Web.Features.Commands.Images;

public class UploadImageCommand : IRequest<string?>
{
    public UploadImageCommand(IFormFile image, Guid userId)
    {
        Image = image;
        UserId = userId;
    }

    public IFormFile Image { get; }
    public Guid UserId { get; }
}

public class UploadImageCommandHandler : IRequestHandler<UploadImageCommand, string?>
{
    private readonly string _webRootPathWithImages;

    public UploadImageCommandHandler(IWebHostEnvironment webHostEnvironment)
    {
        _webRootPathWithImages = webHostEnvironment.WebRootPath + "\\upload\\";
    }

    public async Task<string?> Handle(UploadImageCommand command, CancellationToken token)
    {
        throw new NotImplementedException();
    }
}