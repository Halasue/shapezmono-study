namespace ShapezMono.Game.profile
{
    /// <summary>
    /// 設定が変更されたときに呼び出されるコールバック
    /// </summary>
    /// <param name="app"></param>
    /// <param name="value"></param>
    public delegate void ChangeCallback(Application app, object value);

    /// <summary>
    /// 設定が有効かどうかを判定するコールバック
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public delegate bool EnabledCallback(Application app);

    public class BaseSetting
    {
        public string Id { get; }
        public string CategoryId { get; }
        public Application? App { get; }

        public ChangeCallback ChangeCb { get; }
        public EnabledCallback? EnabledCb { get; }


        public BaseSetting(string id, string categoryId, ChangeCallback changeCb, EnabledCallback? enabledCb = null)
        {
            Id = id;
            CategoryId = categoryId;
            ChangeCb = changeCb;
            EnabledCb = enabledCb;

            App = null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="app"></param>
        /// <param name="value"></param>
        public void Apply(Application app, object value)
        {
            if (ChangeCb != null)
            {
                ChangeCb(app, value);
            }
        }


    }
}
