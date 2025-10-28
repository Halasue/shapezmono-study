using System;
using System.Collections.Generic;
using System.Linq;
using ShapezMono.Game.old;
using SharpDX.WIC;

namespace ShapezMono.Game.old.profile
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

    /// <summary>
    /// 設定項目の基底クラス
    /// </summary>
    public class BaseSetting
    {
        public string Id { get; }
        public string CategoryId { get; }
        public Application? App { get; }

        public ChangeCallback? ChangeCb { get; }
        public EnabledCallback? EnabledCb { get; }


        public BaseSetting(string id, string categoryId, ChangeCallback? changeCb, EnabledCallback? enabledCb = null)
        {
            Id = id;
            CategoryId = categoryId;
            ChangeCb = changeCb;
            EnabledCb = enabledCb;

            App = null;
        }

        /// <summary>
        /// 設定変更を適用し、必要に応じてコールバックを呼び出す
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

        public bool GetIsAvailable(Application app)
        {
            return EnabledCb?.Invoke(app) ?? true;
        }

        public virtual bool Validate(object value)
        {
            return false;
        }

        public class EnumSetting<TOption> : BaseSetting
        {
            public IReadOnlyList<TOption> Options { get; }
            public Func<TOption, string> ValueGetter { get; }
            public Func<TOption, string> TextGetter { get; }
            public Func<TOption, string?> DescGetter { get; }
            public bool RestartRequired { get; }
            public string? IconPrefix { get; }
            public string? MagicValue { get; }

            public EnumSetting(
                string id,
                IReadOnlyList<TOption> options,
                Func<TOption, string> valueGetter,
                Func<TOption, string> textGetter,
                Func<TOption, string?>? descGetter = null,
                string categoryId = "",
                bool restartRequired = true,
                string? iconPrefix = null,
                ChangeCallback? changeCb = null,
                string? magicValue = null,
                EnabledCallback? enabledCb = null
            )
                : base(id, categoryId, changeCb, enabledCb)
            {
                Options = options.ToList();
                ValueGetter = valueGetter;
                TextGetter = textGetter;
                DescGetter = descGetter ?? (_ => null);
                RestartRequired = restartRequired;
                IconPrefix = iconPrefix;
                MagicValue = magicValue;
            }
        }
    }
}
