using PowerDispense.IFactories.IRepoFactories;
using PowerDispense.Interfaces.IRepositories;
using PowerDispense.Models.Enum;
using PowerDispense.Repositories.Power;

namespace PowerDispense.Factories
{
    public class PowerRepoFactory : IPowerRepoFactory
    {
        private readonly IServiceProvider _serviceProvider;
        public PowerRepoFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IPowerRepo GetPowerRepo(DataSource dataSource)
        {
            using var scope = _serviceProvider.CreateScope();
            switch (dataSource)
            {
                case DataSource.Cache:
                    return scope.ServiceProvider.GetRequiredService<RedisPowerRepo>();
                case DataSource.Database:
                    throw new NotImplementedException();
                case DataSource.File:
                    return scope.ServiceProvider.GetRequiredService<FilePowerRepo>();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
