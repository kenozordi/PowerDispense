using PowerDispense.Interfaces.IRepositories;
using PowerDispense.Models.Enum;

namespace PowerDispense.Interfaces.IFactories.IRepoFactories
{
    public interface IPowerRepoFactory
    {
        /// <summary>
        /// Gets the power repository by the datasource
        /// </summary>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        IPowerRepo GetPowerRepo(DataSource dataSource);
    }
}
