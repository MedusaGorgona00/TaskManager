using Application.Common.MediatR;
using Application.Exceptions;
using Application.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Base
{
    
    public abstract class ApiControllerBase : ControllerBase
    {
        private ISender? _mediator;
        private ILogger<ApiControllerBase>? _logger;

        /// <summary>
        ///     MediatR
        /// </summary>
        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>()!;
        protected ILogger<ApiControllerBase> Logger => _logger ??= HttpContext.RequestServices.GetService<ILogger<ApiControllerBase>>()!;

        private IActionResult ThrowErrorIfNotNull(AppException error)
        {
            string? actionName = this.ControllerContext.RouteData.Values["action"]?.ToString();
            string? controllerName = this.ControllerContext.RouteData.Values["controller"]?.ToString();

            if (error is BadRequestException brex) {
                Logger.LogWarning($"BadRequest (400) response in {controllerName}.{actionName}. Message: {brex.Message}");
                return BadRequest(brex.Message); 
            
            }
            if (error is NotFoundException nfex) {
                Logger.LogWarning($"NotFound (404) response in {controllerName}.{actionName}. Message: {nfex.Message}");
                return NotFound(nfex.Message);
            }

            if (error is ForbiddenException fex) {
                Logger.LogWarning($"Forbidden (403) response in {controllerName}.{actionName}. Message: {fex.Message}");
                return Forbid(fex.Message); 
            }

            if (error is AppException appException) {
                Logger.LogWarning($"AppException (500) response in {controllerName}.{actionName}. Message: {appException.Message}");
                return StatusCode(500, appException.Message); 
            }

            return BadRequest("Unexpected exception");
        }

        protected IActionResult ReturnActionResultOrThrowHttpException<T>(ExecResult<T> result, IActionResult actionResult)
        {
            if (result.Error != null)
            {
                return ThrowErrorIfNotNull(result.Error);
            }
            else
            {
                return actionResult;
            }
        }

        protected IActionResult ReturnActionResultOrThrowHttpException(ExecResult result, IActionResult actionResult)
        {
            if (result.Error != null)
            {
                return ThrowErrorIfNotNull(result.Error);
            }
            else
            {
                return actionResult;
            }
        }

        protected IActionResult ReturnOkOrThrowHttpException<T>(ExecResult<T> result)
        {
            if (result.Error != null)
            {
                return ThrowErrorIfNotNull(result.Error);
            }
            else
            {
                return Ok(result.Result);
            }
        }

        protected IActionResult ReturnOkOrThrowHttpException(ExecResult result)
        {
            if (result.Error != null)
            {
                return ThrowErrorIfNotNull(result.Error);
            }
            else
            {
                return Ok();
            }
        }

        protected IActionResult ReturnNoContentOrThrowHttpException(ExecResult result)
        {
            if (result.Error != null)
            {
                return ThrowErrorIfNotNull(result.Error);
            }
            else
            {
                return NoContent();
            }
        }

        protected IActionResult ReturnByteArrayOrThrowHttpException(ExecResult<byte[]> result, string contentType, string? fileName)
        {
            if (result.Error != null)
            {
                return ThrowErrorIfNotNull(result.Error);
            }
            else
            {
                if (!string.IsNullOrEmpty(fileName))
                    return File(result.Result!, contentType, fileName);
                else
                    return File(result.Result!, contentType);
            }
        }
    }
}
