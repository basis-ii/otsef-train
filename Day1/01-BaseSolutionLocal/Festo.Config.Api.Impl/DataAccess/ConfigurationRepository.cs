using Festo.Config.Api.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Festo.Config.Api.Impl.DataAccess
{
    public class ConfigurationRepository : IConfigurationRepository
    {
        private IList<Motor> motors = new List<Motor>();

        public async Task<IList<Motor>> GetMotorsAsync()
        {
            await Task.Delay(10);
            return this.motors;
        }
    }
}
