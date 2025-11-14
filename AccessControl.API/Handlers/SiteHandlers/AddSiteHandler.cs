using AccessControl.API.Enums;
using AccessControl.API.Models;
using AccessControl.API.Services.Infrastructure.LiveEvents;
using Marten;
using MediatR;

namespace AccessControl.API.Handlers.SiteHandlers
{
    public class AddSite
    {
        public class Request : IRequest<Response>
        {
            public Guid UserId { get; set; }
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
                var site = new Site(request.UserId, request.DisplayName);

                _session.Store(site);

                await _liveEventPublisher.PublishAsync(
                    site.SiteId,
                    site.SiteId,
                    "Site",
                    LiveEventMessageType.SiteCreated,
                    site.DisplayName,
                    "Site created");

                await _session.SaveChangesAsync();

                return new Response();
            }
        }
    }
}
