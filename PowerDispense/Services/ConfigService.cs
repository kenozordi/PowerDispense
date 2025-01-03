using Microsoft.Extensions.Options;
using PowerDispense.Models.Config;

namespace PowerDispense.Services
{
    public class ConfigService
    {
        public ConnectionStrings ConnectionStrings;

        public ConfigService(IOptions<ConnectionStrings> connectionStrings)
        {
            ConnectionStrings = connectionStrings.Value;
        }
    }
}
