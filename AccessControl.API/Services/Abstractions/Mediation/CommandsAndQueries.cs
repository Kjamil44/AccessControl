using MediatR;

namespace AccessControl.API.Services.Abstractions.Mediation
{
    public interface ICommand<out TResponse> : IRequest<TResponse> { }
    public interface ICommand : IRequest { }
    public interface IQuery<out TResponse> : IRequest<TResponse> { }
}
