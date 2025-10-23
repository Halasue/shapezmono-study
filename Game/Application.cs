using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using ShapezMono.Game.Core;
using System.IO;

namespace ShapezMono.Game
{
    /// <summary>
    /// メインゲームクラス
    /// </summary>
    public class Application : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Logger _logger;
        private ErrorHandler _errorHandler;

        private Storage _storage;

        public Application()
        {
            _errorHandler = new ErrorHandler();
            _graphics = new GraphicsDeviceManager(this);
            _logger = LoggerFactory.Create(this, ConsoleColor.Cyan);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _logger.Log($"Applicationを作成しました。プラットフォーム: {Utils.GetPlatformName()}");
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        protected override void Initialize()
        {
            boot();
            base.Initialize();
        }

        /// <summary>
        /// コンテンツ読み込み
        /// </summary>
        protected override void LoadContent()
        {
        }

        /// <summary>
        /// 毎フレームの更新処理
        /// </summary>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// 毎フレームの描画処理
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        private void boot()
        {
            _storage = new Storage(this, Storage.STORAGE_SAVES);
            _storage.Initialize();
        }
    }
}
