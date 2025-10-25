using System.Collections.Generic;
using ShapezMono.Game.Core;

namespace ShapezMono.Game.profile
{
    public class ApplicationSettings : ReadWriteProxy
    {
        private readonly Application _app;

        public List<BaseSetting> SettingHandles;

        public ApplicationSettings(Application app, Storage storage) : base(storage, "app_settings.dat")
        {
            _app = app;
            SettingHandles = InitializeSettings();
        }

        public void Initialize()
        {

        }

        private static List<BaseSetting> InitializeSettings()
        {
            return new List<BaseSetting>
            {
            };
        }
    }
}
