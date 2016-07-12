using Festo.Config.Api.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Festo.Config.Api.Impl.DataAccess
{
    public interface IConfigurationRepository
    {
        Task<IList<Motor>> GetMotorsAsync();
    }
}
