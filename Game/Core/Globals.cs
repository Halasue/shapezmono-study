namespace ShapezMono.Game.Core
{
    public static class BuildOptions
    {
        public static readonly bool IsDev =
#if DEBUG
            true;
#else
            false;
#endif
        public static readonly bool IsRelease = !IsDev;
    }
}
