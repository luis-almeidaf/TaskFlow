using MediatR;

namespace TaskFlow.Application.Features.Auth.Commands.Logout;

public class LogoutCommand : IRequest<Unit> { }