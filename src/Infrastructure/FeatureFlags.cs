namespace V1_Trade.Infrastructure
{
    public static class FeatureFlags
    {
        public static bool AdvancedOcxHost { get; set; } = false;
        public static bool UIThrottle { get; set; } = true;
        public static bool GridDiff { get; set; } = true;
        public static bool PerfHUD { get; set; } = true;
        public static bool OrderGuards { get; set; } = true;
    }
}
