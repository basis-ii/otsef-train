using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Festo.Config.Api.Impl;
using System.Net.Http;
using System.Threading.Tasks;
using Festo.Config.Api.Model;
using Owin;
using System.Web.Http;
using System.ComponentModel.Composition.Registration;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Festo.Config.Api.Impl.DataAccess;
using Festo.Config.Api.Impl.DataAccess.Fakes;
using System.Collections.Generic;

namespace Festo.Config.Tests
{
    [TestClass]
    public class MotorControllerTest
    {
        private class StartupMock
        {
            public void Configuration(IAppBuilder app)
            {
                var config = new HttpConfiguration();
                config.MapHttpAttributeRoutes();

                var rb = new RegistrationBuilder();
                rb.ForTypesMatching<ApiController>(t => typeof(ApiController).IsAssignableFrom(t) && t.Name.EndsWith("Controller"))
                    .Export()
                    .SetCreationPolicy(CreationPolicy.NonShared);
                var catalog = new AssemblyCatalog(typeof(Startup).Assembly, rb);
                var container = new CompositionContainer(catalog);
                container.ComposeExportedValue<IConfigurationRepository>(new StubIConfigurationRepository
                {
                    GetMotorsAsync = () => Task.FromResult<IList<Motor>>(
                        new [] { new Motor {  ID = 1, Description = "Test", MaxCurrent = 42.0f } })
                });
                config.DependencyResolver = new MefDependencyResolver(container);

                app.UseWebApi(config);
            }
        }

        [TestMethod]
        public async Task GetGetAllMotors()
        {
            using (Microsoft.Owin.Hosting.WebApp.Start<StartupMock>("http://localhost:2345"))
            {
                using (var client = new HttpClient())
                {
                    var result = await client.GetAsync("http://localhost:2345/api/motor");
                    result.EnsureSuccessStatusCode();
                    var motors = await result.Content.ReadAsAsync<Motor[]>();
                    Assert.AreEqual(1, motors.Length);
                    Assert.AreEqual("Test", motors[0].Description);
                }
            }
        }
    }
}
