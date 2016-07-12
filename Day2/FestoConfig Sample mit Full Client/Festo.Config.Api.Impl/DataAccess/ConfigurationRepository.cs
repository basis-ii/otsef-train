using Festo.Config.Api.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Festo.Config.Api.Impl.DataAccess
{
    public class ConfigurationRepository : IConfigurationRepository
    {
        internal IList<Motor> motors = new List<Motor>()
        {
            new Motor { ID = 1, Description = "Test 1", MaxCurrent = 42.0f },
            new Motor { ID = 2, Description = "Test 2", MaxCurrent = 43.0f },
            new Motor { ID = 3, Description = "Test 3", MaxCurrent = 44.0f }
        };

        public async Task<IList<Motor>> GetMotorsAsync()
        {
            await Task.Delay(10);
            return this.motors;
        }
    }
}
