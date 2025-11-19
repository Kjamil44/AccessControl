using AccessControl.API.Enums;
using AccessControl.API.Exceptions;
using AccessControl.API.Models;
using AccessControl.API.Services.Abstractions.Mediation;
using AccessControl.API.Services.Infrastructure.LiveEvents;
using Marten;
using MediatR;

namespace AccessControl.API.Handlers.SiteHandlers
{
    public class DeleteSite
    {
        public class Request : ICommand<Response>
        {
            public Guid SiteId { get; set; }
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
                var site = await _session.Query<Site>()
                    .FirstOrDefaultAsync(x => x.SiteId == request.SiteId);

                if (site == null)
                    throw new CoreException("Site not found");

                var lockIsPresent = await _session.Query<Lock>()
                    .AnyAsync(x => x.SiteId == request.SiteId);

                if (lockIsPresent)
                    throw new CoreException("Failed to delete Site: Lock is present");

                var cardholderIsPresent = await _session.Query<Cardholder>()
                  .AnyAsync(x => x.SiteId == request.SiteId);

                if (cardholderIsPresent)
                    throw new CoreException("Failed to delete Site: Cardholder is present");

                var scheduleIsPresent = await _session.Query<Schedule>()
                  .AnyAsync(x => x.SiteId == request.SiteId);

                if (scheduleIsPresent)
                    throw new CoreException("Failed to delete Site: Schedule is present");

                _session.Delete(site);

                await _liveEventPublisher.PublishAsync(
                     site.SiteId,
                     site.SiteId,
                     "Site",
                     LiveEventMessageType.SiteDeleted,
                     site.DisplayName,
                     "Site deleted");

                return new Response();
            }
        }
    }
}
