using API.Controllers.Base;
using Application.Features.ReportContext.Dto;
using Application.Features.ReportContext.Queries;
using Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
[Route("api/reports")]
public class ReportsController : ApiControllerBase
{
    /// <summary>
    /// Get report from migrated EF LINQ
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(IList<ReportDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUsers([FromQuery] GetReportQuery query)
    {
        var result = await Mediator.Send(query);

        return Ok(result);
    }
}
