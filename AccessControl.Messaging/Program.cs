using MassTransit;
using AccessControl.Messaging.Consumers;

Host.CreateDefaultBuilder(args)
  .ConfigureServices((ctx, services) =>
  {
      services.AddMassTransit(x =>
      {
          x.AddConsumer<TriggerLockConsumer>();

          x.UsingRabbitMq((context, cfg) =>
          {
              cfg.Host("localhost", "/", h => { h.Username("guest"); h.Password("guest"); });

              cfg.ReceiveEndpoint("trigger-lock", e =>
              {
                  e.ConfigureConsumer<TriggerLockConsumer>(context);
              });
          });
      });
  })
  .Build()
  .Run();
