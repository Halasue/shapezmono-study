// ApplicationSettings.cs
// 本家は各設定のGetterをそれぞれ作っているが、
// C#のプロパティ機能を利用して直接アクセスする形に変更。
// うおおJSONシリアライズ最強！！

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShapezMono.Game.old;
using ShapezMono.Game.old.Core;

namespace ShapezMono.Game.old.profile
{
    public class ApplicationSettingsData : Data
    {
        public SettingsStorage Settings { get; set; } = new();
    }

    public class ApplicationSettings : ReadWriteProxy<ApplicationSettingsData>
    {
        private readonly Application _app;

        public List<BaseSetting> SettingHandles;

        public ApplicationSettings(Application app, Storage storage) : base(storage, "app_settings.dat")
        {
            _app = app;
            SettingHandles = InitializeSettings();
        }

        public async Task Initialize()
        {
            await ReadAsync();
            var settings = GetAllSettings();
            foreach (var handle in SettingHandles)
            {
                // handle.Apply(_app, settings[handle.Id]);
            }
            await WriteAsync();
        }

        /// <summary>
        /// 設定を保存する
        /// </summary>
        /// <returns></returns>
        public Task Save()
        {
            return WriteAsync();
        }

        /// <summary>
        /// 互換性用エイリアス
        /// すべての設定を取得する
        /// </summary>
        /// <returns></returns>
        public SettingsStorage? GetAllSettings()
        {
            if (CurrentData?.Settings == null) return null;
            return CurrentData.Settings;
        }

        protected override ApplicationSettingsData GetDefaultData()
        {
            return new ApplicationSettingsData
            {
                Version = GetCurrentVersion(),
                Settings = new SettingsStorage()
            };
        }

        protected override int GetCurrentVersion()
        {
            return 1;
        }


        private static List<BaseSetting> InitializeSettings()
        {
            return new List<BaseSetting>
            {
            };
        }
    }
}
