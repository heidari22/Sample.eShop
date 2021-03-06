﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using Yocale.eShop.Utility.Data;

namespace Yocale.eShop.WebAPI.Controllers.BaseAPI
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        private readonly ILogger<BaseApiController> _logger;
        public BaseApiController(ILoggerFactory loggerFactory) => _logger = loggerFactory.CreateLogger<BaseApiController>();

        protected IActionResult ApiResult<T>(ResultModel<T> model)
        {
            if (model == null)
            {
                _logger.LogInformation($"An error accured");

                return StatusCode((int)HttpStatusCode.InternalServerError, "An ereor accured");
            }
            else if (model.Error != null)
            {
                _logger.LogInformation($"User error: {model.Error.Message}, status code: {model.Error.Code}");

                if (model.Error.Code == (int)HttpStatusCode.BadRequest)
                    return BadRequest(model.Error.Message);
                if (model.Error.Code == (int)HttpStatusCode.Forbidden)
                    return Forbid(model.Error.Message);
                if (model.Error.Code == (int)HttpStatusCode.Unauthorized)
                    return Unauthorized(model.Error.Message);
                else if (model.Error.Code == (int)HttpStatusCode.NotFound)
                    return NotFound(model.Error.Message);
                else
                    return StatusCode((int)HttpStatusCode.InternalServerError, model.Error.Message);
            }
            else
            {
                return Ok(model.Data);
            }
        }
    }
}
