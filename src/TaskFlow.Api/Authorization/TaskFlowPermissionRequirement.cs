using Microsoft.AspNetCore.Authorization;

namespace TaskFlow.Api.Authorization;

public class TaskFlowPermissionRequirement(string permission) : IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}