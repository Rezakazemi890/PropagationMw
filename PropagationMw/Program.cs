using PropagationMw;
using PropagationMw.Clients;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient<ISampleClient, SampleClient>()
    .AddHttpMessageHandler(provider =>
    {
        var accessor = provider.GetRequiredService<IHttpContextAccessor>();
        string[] headersToPropagate = ["X-Correlation-ID", "Authorization", "X-Request-ID"];
        return new SafeHeaderPropagationHandler(accessor, headersToPropagate);
    });

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();