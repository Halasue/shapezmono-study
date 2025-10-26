using System.Collections.Generic;
using System.Threading.Tasks;
using ShapezMono.Game.Core;

namespace ShapezMono.Game.profile
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
                handle.Apply(_app, settings[handle.Id]);
            }
            await WriteAsync();
        }

        public SettingsStorage GetAllSettings()
        {
            return CurrentData.Settings;
        }

        private static List<BaseSetting> InitializeSettings()
        {
            return new List<BaseSetting>
            {
            };
        }
    }
}
