using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Repositories.Board;
using TaskFlow.Domain.Repositories.User;
using TaskFlow.Exception;

namespace TaskFlow.Api.Authorization;

public class TaskFlowPermissionHandler(
    IBoardReadOnlyRepository boardRepository,
    IHttpContextAccessor httpContextAccessor,
    IUserReadOnlyRepository userRepository) : AuthorizationHandler<TaskFlowPermissionRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        TaskFlowPermissionRequirement requirement)
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext is null)
        {
            context.Fail();
            return;
        }

        var userIdClaim = context.User.FindFirst(ClaimTypes.Sid);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            context.Fail();
            return;
        }

        var user = await userRepository.GetById(userId);
        if (user is null)
        {
            context.Fail(new AuthorizationFailureReason(this, ResourceErrorMessages.USER_NOT_FOUND));
            return;
        }

        var routeData = httpContext.GetRouteData();
        if (!routeData.Values.TryGetValue("boardId", out var boardIdObj) ||
            !Guid.TryParse(boardIdObj?.ToString(), out var boardId))
        {
            context.Fail();
            return;
        }

        var board = await boardRepository.GetById(boardId);
        if (board is null)
        {
            context.Fail(new AuthorizationFailureReason(this, ResourceErrorMessages.BOARD_NOT_FOUND));
            return;
        }

        var boardMember = await boardRepository.GetBoardMember(board.Id, user.Id);
        if (boardMember == null)
        {
            context.Fail(new AuthorizationFailureReason(this, ResourceErrorMessages.USER_NOT_IN_BOARD));
            return;
        }

        var userRole = boardMember.Role;

        var hasPermission = HasPermission(requirement, userRole);

        if (hasPermission)
            context.Succeed(requirement);
        else
            context.Fail();
    }

    private static bool HasPermission(TaskFlowPermissionRequirement requirement, BoardRole? userRole)
    {
        var hasPermission = requirement.Permission switch
        {
            TaskFlowPermissions.Boards.Delete => userRole == BoardRole.Owner,
            TaskFlowPermissions.Boards.Update => userRole == BoardRole.Owner || userRole == BoardRole.Admin,
            TaskFlowPermissions.Boards.AddBoardMember => userRole == BoardRole.Owner || userRole == BoardRole.Admin,
            TaskFlowPermissions.Boards.RemoveBoardMember => userRole == BoardRole.Owner || userRole == BoardRole.Admin,

            TaskFlowPermissions.Columns.Create => userRole == BoardRole.Owner || userRole == BoardRole.Admin,
            TaskFlowPermissions.Columns.Update => userRole == BoardRole.Owner || userRole == BoardRole.Admin,
            TaskFlowPermissions.Columns.Delete => userRole == BoardRole.Owner || userRole == BoardRole.Admin,
            TaskFlowPermissions.Columns.Move => userRole == BoardRole.Owner || userRole == BoardRole.Admin,

            TaskFlowPermissions.Cards.Create => userRole == BoardRole.Owner || userRole == BoardRole.Admin,
            TaskFlowPermissions.Cards.Update => userRole == BoardRole.Owner || userRole == BoardRole.Admin,
            TaskFlowPermissions.Cards.Delete => userRole == BoardRole.Owner || userRole == BoardRole.Admin,
            TaskFlowPermissions.Cards.Assign => userRole == BoardRole.Owner || userRole == BoardRole.Admin,
            _ => false
        };
        return hasPermission;
    }
}