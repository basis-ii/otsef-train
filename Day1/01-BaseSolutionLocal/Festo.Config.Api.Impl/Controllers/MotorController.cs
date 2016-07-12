using Festo.Config.Api.Impl.DataAccess;
using Festo.Config.Api.Model;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.ComponentModel.Composition;

namespace Festo.Config.Api.Impl.Controllers
{
    [RoutePrefix("api")]
    public class MotorController : ApiController
    {
        private IConfigurationRepository repository;

        [ImportingConstructor]
        public MotorController(IConfigurationRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        [Route("motor")]
        public async Task<IEnumerable<Motor>> Get()
        {
            return await repository.GetMotorsAsync();
        }

        [HttpGet]
        [Route("motor/{id}")]
        public async Task<IHttpActionResult> GetById(int id)
        {
            var motor = (await repository.GetMotorsAsync())
                .SingleOrDefault(m => m.ID == id);

            if (motor == null)
            {
                return this.NotFound();
            }

            return this.Ok(motor);
        }

        [HttpPost]
        [Route("motor")]
        public async Task<IHttpActionResult> Insert([FromBody] Motor m)
        {
            // ToDo: Check for duplicate IDs

            (await repository.GetMotorsAsync()).Add(m);
            return this.Created($"/api/motor/{m.ID}", m);
        }

        [HttpDelete]
        [Route("motor/{id}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            var motor = (await repository.GetMotorsAsync())
                            .SingleOrDefault(m => m.ID == id);

            if (motor == null)
            {
                return this.NotFound();
            }

            (await repository.GetMotorsAsync()).Remove(motor);
            return this.StatusCode(HttpStatusCode.NoContent);
        }
    }
}
