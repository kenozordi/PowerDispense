using PowerDispense.Interfaces.IFactories;
using PowerDispense.Models.Constants;
using PowerDispense.Models.Enum;

namespace PowerDispense.Services
{
    public class ProviderFactory : IPowerProviderFactory
    {
        public string GetProviderStreamKey(PowerProvider powerProvider)
        {
            switch (powerProvider)
            {
                case PowerProvider.AEDC:
                    return CacheKey.STREAM_AEDC;
                case PowerProvider.EKEDC:
                    return CacheKey.STREAM_EKEDC;
                default:
                    throw new NotImplementedException();
            }
        }
        public string GetProviderChannel(PowerProvider powerProvider)
        {
            switch (powerProvider)
            {
                case PowerProvider.AEDC:
                    return $"{CacheKey.CHANNEL_TRANS}:{powerProvider.ToString()}";
                case PowerProvider.EKEDC:
                    return CacheKey.STREAM_EKEDC;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
