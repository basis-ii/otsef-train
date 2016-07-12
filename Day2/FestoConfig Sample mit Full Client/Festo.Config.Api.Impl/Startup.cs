using Festo.Config.Api.Impl.DataAccess;
using Festo.Config.Api.Model;
using Owin;
using Swashbuckle.Application;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Registration;
using System.Net;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;

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
            config.EnableSwagger(c => c.SingleApiVersion("v1", "Great Festo Configuration API"))
                .EnableSwaggerUi();
            config.MapHttpAttributeRoutes();
            var builder = new ODataConventionModelBuilder();
            var motorSet = builder.EntitySet<Motor>("Motor");
            config.MapODataServiceRoute(
                routeName: "odata",
                routePrefix: "odata",
                model: builder.GetEdmModel());

            var rb = new RegistrationBuilder();
            rb.ForTypesMatching<ApiController>(t => typeof(ApiController).IsAssignableFrom(t) && t.Name.EndsWith("Controller"))
                .Export()
                .SetCreationPolicy(CreationPolicy.NonShared);
            rb.ForTypesMatching<ODataController>(t => typeof(ODataController).IsAssignableFrom(t) && t.Name.EndsWith("Controller"))
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
