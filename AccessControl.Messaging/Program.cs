using MassTransit;
using AccessControl.Messaging.Consumers;

Host.CreateDefaultBuilder(args)
  .ConfigureServices((ctx, services) =>
  {
      var cfg = ctx.Configuration;
      services.AddMassTransit(x =>
      {
          x.SetKebabCaseEndpointNameFormatter();
          x.AddConsumer<UnlockDoorConsumer>();
          //TODO: ADD MORE CONSUMERS HERE

          x.UsingRabbitMq((context, bus) =>
          {
              bus.Host(cfg["Rabbit:Host"] ?? "localhost", cfg["Rabbit:VHost"] ?? "/", h =>
              {
                  h.Username(cfg["Rabbit:User"] ?? "admin");
                  h.Password(cfg["Rabbit:Pass"] ?? "admin");
              });
              bus.UseMessageRetry(r => r.Incremental(5, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(5)));
              bus.UseInMemoryOutbox();
              bus.ConfigureEndpoints(context);
          });
      });
  })
  .Build()
  .Run();
