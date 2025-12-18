using Microsoft.AspNetCore.Authorization;
using TaskFlow.Api.Authorization;

namespace TaskFlow.Api;

public static class DependencyInjectionExtension
{
    public static void AddTaskFlowPolicies(this AuthorizationBuilder builder)
    {
        builder
            .AddPolicy("boards:update", p => p.Requirements.Add(new TaskFlowPermissionRequirement(TaskFlowPermissions.Boards.Update)))
            .AddPolicy("boards:delete", p => p.Requirements.Add(new TaskFlowPermissionRequirement(TaskFlowPermissions.Boards.Delete)))
            .AddPolicy("boards:add-member", p => p.Requirements.Add(new TaskFlowPermissionRequirement(TaskFlowPermissions.Boards.AddBoardMember)))
            .AddPolicy("boards:remove-member", p => p.Requirements.Add(new TaskFlowPermissionRequirement(TaskFlowPermissions.Boards.RemoveBoardMember)))
            
            .AddPolicy("columns:create", p => p.Requirements.Add(new TaskFlowPermissionRequirement(TaskFlowPermissions.Columns.Create)))
            .AddPolicy("columns:update", p => p.Requirements.Add(new TaskFlowPermissionRequirement(TaskFlowPermissions.Columns.Update)))
            .AddPolicy("columns:delete", p => p.Requirements.Add(new TaskFlowPermissionRequirement(TaskFlowPermissions.Columns.Delete)))
            .AddPolicy("columns:move", p => p.Requirements.Add(new TaskFlowPermissionRequirement(TaskFlowPermissions.Columns.Move)))
            
            .AddPolicy("cards:create", p => p.Requirements.Add(new TaskFlowPermissionRequirement(TaskFlowPermissions.Cards.Create)))
            .AddPolicy("cards:update", p => p.Requirements.Add(new TaskFlowPermissionRequirement(TaskFlowPermissions.Cards.Update)))
            .AddPolicy("cards:delete", p => p.Requirements.Add(new TaskFlowPermissionRequirement(TaskFlowPermissions.Cards.Delete)))
            .AddPolicy("cards:assign", p => p.Requirements.Add(new TaskFlowPermissionRequirement(TaskFlowPermissions.Cards.Assign)));
    }
}