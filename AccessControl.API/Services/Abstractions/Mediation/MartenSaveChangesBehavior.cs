using MediatR;
using Marten;

namespace AccessControl.API.Services.Abstractions.Mediation
{
    public sealed class MartenSaveChangesBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IDocumentSession _session;

        public MartenSaveChangesBehavior(IDocumentSession session) => _session = session;

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken ct)
        {
            var response = await next();

            if (request is ICommand || request is ICommand<TResponse>)
            {
                await _session.SaveChangesAsync(ct);
            }

            return response;
        }
    }

}
