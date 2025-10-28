using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShapezMono.Game
{
    public class MainGame : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch? _spriteBatch;

        private StateManager _stateManager;
        private Logger _logger;

        public MainGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _logger = LoggerFactory.Create("MainGame");
            _stateManager = new StateManager();
        }

        protected override void Initialize()
        {
            base.Initialize();

            _logger.Log("MainGame 初期化完了");

            _stateManager.Register("Preload", new State.PreloadState());
            _stateManager.MoveTo("Preload");
        }
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _stateManager.Current?.OnRender(dt);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }
    }
}
