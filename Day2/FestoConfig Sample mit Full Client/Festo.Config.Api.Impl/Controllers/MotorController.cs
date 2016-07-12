using Festo.Config.Api.Impl.DataAccess;
using Festo.Config.Api.Model;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using System.Web.OData;
using System.Web.OData.Routing;

namespace Festo.Config.Api.Impl.Controllers
{
    [ODataRoutePrefix("Motor")]
    public class MotorController : ODataController
    {
        private IConfigurationRepository repository;

        [ImportingConstructor]
        public MotorController(IConfigurationRepository repository)
        {
            this.repository = repository;
        }

        [EnableQuery]
        [ODataRoute]
        public IQueryable<Motor> Get()
        {
            // Achtung! HACK!!
            return ((ConfigurationRepository)repository).motors.AsQueryable();
        }
    }
}
