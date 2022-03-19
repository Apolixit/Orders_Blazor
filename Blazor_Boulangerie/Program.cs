using Blazor_Orders;
using Blazor_Orders.Helpers;
using Blazor_Orders.Repository;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

ConfigureServices(builder.Services);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();

static void ConfigureServices(IServiceCollection services)
{
    services.AddScoped<IHttpService, HttpService>();
    services.AddScoped<IScheduleRepository, ScheduleRepository>();
}