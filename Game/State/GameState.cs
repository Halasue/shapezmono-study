namespace ShapezMono.Game.State
{
    /// <summary>
    /// ゲームの状態を表す抽象クラス
    /// 本家Shapezの
    /// </summary>
    public abstract class GameState
    {
        public abstract void OnRender(float deltaTime);
        public virtual void OnResized(int width, int height) { }
        public virtual void OnAppPause() { }
        public virtual void OnAppResume() { }
        public virtual bool GetIsInGame() => false;
        public virtual void OnBackgroundTick() { }
    }
}
