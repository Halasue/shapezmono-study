namespace ShapezMono.Game.State
{
    public class PreloadState : GameState
    {
        private Logger _logger;

        public PreloadState()
        {
            _logger = LoggerFactory.Create("PreloadState");
        }

        public override void OnRender(float deltaTime)
        {
            _logger.Log($"Rendering frame dt={deltaTime:F4}");
        }
    }
}
