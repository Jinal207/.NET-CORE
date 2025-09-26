using Microsoft.AspNetCore.Mvc;

[Route("/error")]
public class ErrorController : ControllerBase
{
    [HttpGet("hidden-action")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpGet]
    public IActionResult HandleError()
    {
        return Problem(
            title: "An unexpected error occurred from error controller.",
            statusCode: StatusCodes.Status500InternalServerError
        );
    }
}