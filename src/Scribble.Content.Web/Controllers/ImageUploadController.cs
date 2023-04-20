using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Scribble.Content.Web.Controllers;

[ApiController]
[Route("api/{version:apiVersion}/images")]
public class ImageUploadController : ControllerBase
{
    private readonly IMediator _mediator;

    public ImageUploadController(IMediator mediator) => _mediator = mediator;

    // public async Task<IActionResult> UploadImage(IFormFile image)
    // {
    //     throw new NotImplementedException();
    // }
}