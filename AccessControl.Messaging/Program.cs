using MassTransit;
using AccessControl.Messaging.Consumers;

Host.CreateDefaultBuilder(args)
  .ConfigureServices((ctx, services) =>
  {
      services.AddMassTransit(x =>
      {
          x.AddConsumer<LockTriggerConsumer>();
          x.AddConsumer<UnlockTriggerConsumer>();

          x.UsingRabbitMq((context, cfg) =>
          {
              cfg.Host("localhost", "/", h => { h.Username("guest"); h.Password("guest"); });

              cfg.ReceiveEndpoint("trigger-lock", e =>
              {
                  e.ConfigureConsumer<LockTriggerConsumer>(context);
              });

              cfg.ReceiveEndpoint("trigger-unlock", e =>
              {
                  e.ConfigureConsumer<UnlockTriggerConsumer>(context);
              });
          });
      });
  })
  .Build()
  .Run();
