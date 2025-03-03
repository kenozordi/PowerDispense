namespace PowerDispense.Models.Constants
{
    public static class CacheKey
    {
        public const string Meter = "Meter";
        public const string Raffle = "Raffle";
        public const int RaffleExpiryInHours = 24;
        public const string Status = "Status";
        public const string Draw = "Draw";

        public const string POWER_PROVIDER_STABLE = "PowerProviders:Stable";
        public const string POWER_PROVIDER_UNSTABLE = "PowerProviders:Unstable";

        // Stream
        public const string STREAM_EKEDC = "Stream:Transaction:EKEDC";
        public const string STREAM_AEDC = "Stream:Transaction:AEDC";

        // Channel
        public const string CHANNEL_TRANS = "Channel:Transaction";

        // Consumer Group
        public const string CONSUMER_GROUP = "Stream:Consumer:Group";
    }
}
