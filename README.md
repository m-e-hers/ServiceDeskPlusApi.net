# Usage examples
## MVC DIExtensions

#### Program.cs
```cs

  builder.Services.AddSdpApiDataSource(builder.Configuration);
```

#### ServiceDeskPlusApiDiExtensions.cs
```cs
using ManageEngine;

namespace HelpdeskExtensions.Api.DiExtensions
{
    public static class ServiceDeskPlusApiDiExtensions
    {
        public static IServiceCollection AddSdpApiDataSource(this IServiceCollection services, IConfiguration config)
        {
            var meDomain = config.GetValue<string>(Constants.EnvironmentVariableNames.MEURL)
                ?? throw new InvalidOperationException($"{Constants.EnvironmentVariableNames.MEURL} Can Not Be Empty!");

            var meTechKey = config.GetValue<string>(Constants.EnvironmentVariableNames.METechKey)
                ?? throw new InvalidOperationException($"{Constants.EnvironmentVariableNames.METechKey} Can Not Be Empty!");

            services.AddTransient(s =>
            {
                return new ServiceDeskPlusApi(meDomain, meTechKey);
            });
            return services;
        }
    }
}
```

#### RandomController.cs
```cs
namespace HelpdeskExtensions.Api.Controllers
{
    [ApiController]
    public class MEJiraController : ControllerBase
    {
        private readonly ServiceDeskPlusApi _sdpApi;

        public MEJiraController(
            ServiceDeskPlusApi sdpApi)
        {
            _sdpApi = sdpApi;
        }

//...
    Request woRequest = await _sdpApi.Request.View(woRequestId);
//...
```
