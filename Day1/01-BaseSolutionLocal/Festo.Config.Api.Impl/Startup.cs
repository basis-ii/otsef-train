using Festo.Config.Api.Impl.DataAccess;
using Owin;
using Swashbuckle.Application;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Registration;
using System.Net;
using System.Web.Http;

namespace Festo.Config.Api.Impl
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Use(async (context, next) =>
            {
                if (context.Request.Headers["x-festo-version"] == "0.0.1")
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
                else
                {
                    await next();
                }
            });

            var config = new HttpConfiguration();
            config.EnableSwagger(c => c.SingleApiVersion("v1", "Festo Configuration API"))
                .EnableSwaggerUi();
            config.MapHttpAttributeRoutes();

            var rb = new RegistrationBuilder();
            rb.ForTypesMatching<ApiController>(t => typeof(ApiController).IsAssignableFrom(t) && t.Name.EndsWith("Controller"))
                .Export()
                .SetCreationPolicy(CreationPolicy.NonShared);
            var catalog = new AssemblyCatalog(typeof(Startup).Assembly, rb);
            var container = new CompositionContainer(catalog);
            container.ComposeExportedValue<IConfigurationRepository>(new ConfigurationRepository());
            config.DependencyResolver = new MefDependencyResolver(container);

            app.UseWebApi(config);
        }
    }
}
