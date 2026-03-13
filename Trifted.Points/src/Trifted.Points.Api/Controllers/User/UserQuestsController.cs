using Kanject.Core.Api.Abstractions.Models;
using Kanject.Core.ApiV2.Controller;
using Kanject.Identity.Abstractions.Security.SystemPermissions.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trifted.Points.Business.Services.WdrbeQuest.Abstractions.Interfaces;
using Trifted.Points.Business.Services.WdrbeQuest.Abstractions.Models;


namespace Trifted.Points.Api.Controllers.User;

/// <summary>
/// Controller for managing Wdrbe quests and event topics within the admin module.
/// </summary>
/// <remarks>
/// The <c>WdrbeQuestsController</c> provides endpoints to interact with the Wdrbe quests, including
/// retrieving available event topics and creating new quests.
/// </remarks>
[Route("api/user/wdrbe-quests")]
[Authorize]
[ApiController]
[Module("wdrbe-quests")]
public class UserQuestsController(IWdrbeQuestManagerService wdrbeQuestManagerService) : BaseController
{
    /// <summary>
    /// Get quest of a user
    /// </summary>
    /// <returns></returns>
    [HttpGet()]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Response<GetWdrbeQuestTasksResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUsersQuestPointAsync()
    {
        var payload = await wdrbeQuestManagerService.GetUsersQuestPointAsync(CurrentUserId);

        return wdrbeQuestManagerService.HasError
            ? ApiErrorResponse(wdrbeQuestManagerService.Errors)
            : ApiResponse(payload);
    }

}