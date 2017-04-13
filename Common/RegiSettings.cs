namespace Common
{
    public static class RegiSettings
    {
        public static string REGISTER_ENABLED = "User Registration";
        public static bool REGISTER_ENABLED_DEFAULT = true;

#if DEBUG
        public static string BLOCKCHAIN_HOST = "localhost";
#else
        public static string BLOCKCHAIN_HOST = "api.evoto.tech";
#endif
    }
}