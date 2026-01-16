using Microsoft.AspNetCore.Authorization;
using TaskFlow.Api.Authorization;

namespace TaskFlow.Api;

public static class DependencyInjectionExtension
{
    public static void AddTaskFlowPolicies(this AuthorizationBuilder builder)
    {
        builder
            .AddPolicy(TaskFlowPermissions.Boards.Update, p => p.Requirements.Add(new TaskFlowPermissionRequirement(TaskFlowPermissions.Boards.Update)))
            .AddPolicy(TaskFlowPermissions.Boards.Delete, p => p.Requirements.Add(new TaskFlowPermissionRequirement(TaskFlowPermissions.Boards.Delete)))
            .AddPolicy(TaskFlowPermissions.Boards.AddBoardMember, p => p.Requirements.Add(new TaskFlowPermissionRequirement(TaskFlowPermissions.Boards.AddBoardMember)))
            .AddPolicy(TaskFlowPermissions.Boards.RemoveBoardMember, p => p.Requirements.Add(new TaskFlowPermissionRequirement(TaskFlowPermissions.Boards.RemoveBoardMember)))
            .AddPolicy(TaskFlowPermissions.Boards.ChangeBoardMemberRole, p => p.Requirements.Add(new TaskFlowPermissionRequirement(TaskFlowPermissions.Boards.ChangeBoardMemberRole)))

            .AddPolicy(TaskFlowPermissions.Columns.Create, p => p.Requirements.Add(new TaskFlowPermissionRequirement(TaskFlowPermissions.Columns.Create)))
            .AddPolicy(TaskFlowPermissions.Columns.Update, p => p.Requirements.Add(new TaskFlowPermissionRequirement(TaskFlowPermissions.Columns.Update)))
            .AddPolicy(TaskFlowPermissions.Columns.Delete, p => p.Requirements.Add(new TaskFlowPermissionRequirement(TaskFlowPermissions.Columns.Delete)))
            .AddPolicy(TaskFlowPermissions.Columns.Move, p => p.Requirements.Add(new TaskFlowPermissionRequirement(TaskFlowPermissions.Columns.Move)))

            .AddPolicy(TaskFlowPermissions.Cards.Create, p => p.Requirements.Add(new TaskFlowPermissionRequirement(TaskFlowPermissions.Cards.Create)))
            .AddPolicy(TaskFlowPermissions.Cards.Update, p => p.Requirements.Add(new TaskFlowPermissionRequirement(TaskFlowPermissions.Cards.Update)))
            .AddPolicy(TaskFlowPermissions.Cards.Delete, p => p.Requirements.Add(new TaskFlowPermissionRequirement(TaskFlowPermissions.Cards.Delete)))
            .AddPolicy(TaskFlowPermissions.Cards.Assign, p => p.Requirements.Add(new TaskFlowPermissionRequirement(TaskFlowPermissions.Cards.Assign)));
    }
}