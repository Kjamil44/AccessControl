using AccessControl.API.Enums;
using AccessControl.API.Exceptions;
using AccessControl.API.Models;
using AccessControl.API.Services.Abstractions.Mediation;
using AccessControl.API.Services.Infrastructure.LiveEvents;
using Marten;
using MediatR;

namespace AccessControl.API.Handlers.SiteHandlers
{
    public class UpdateSite
    {
        public class Request : ICommand<Response>
        {
            public Guid SiteId { get; set; }
            public string DisplayName { get; set; }
        }
        public class Response
        {
        }
        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IDocumentSession _session;
            private readonly ILiveEventPublisher _liveEventPublisher;


            public Handler(IDocumentSession session, ILiveEventPublisher liveEventPublisher)
            {
                _session = session;
                _liveEventPublisher = liveEventPublisher;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var site = await _session.LoadAsync<Site>(request.SiteId);
                if (site == null)
                    throw new CoreException("Site not found");

                site.UpdateSite(request.DisplayName);
                _session.Store(site);

                await _liveEventPublisher.PublishAsync(
                    site.SiteId,
                    site.SiteId,
                    "Site",
                    LiveEventMessageType.SiteUpdated,
                    site.DisplayName,
                    "Site name was updated");

                return new Response();
            }
        }
    }
}
