using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;
using SharpDX.Direct2D1.Effects;
using static ShapezMono.Game.old.profile.SettingsStorage;

namespace ShapezMono.Game.old.profile
{
    public class SettingsStorage
    {
        // ======================== 列挙型定義 ========================

        /// <summary>
        /// UIスケールID
        /// </summary>
        public enum UiScaleId
        {
            SuperSmall,
            Small,
            Regular,
            Large,
            Huge
        }

        /// <summary>
        /// スクロールホイール感度ID
        /// </summary>
        public enum ScrollWheelSensitivityId
        {
            SuperSlow,
            Slow,
            Regular,
            Fast,
            SuperFast
        }

        /// <summary>
        /// 移動速度ID
        /// </summary>
        public enum MovementSpeedId
        {
            SuperSlow,
            Slow,
            Regular,
            Fast,
            SuperFast,
            ExtremelyFast
        }

        /// <summary>
        /// オートセーブ間隔ID
        /// </summary>
        public enum AutosaveIntervalId
        {
            OneMinute,
            TwoMinutes,
            FiveMinutes,
            TenMinutes,
            TwentyMinutes,
            Disabled
        }

        /// <summary>
        /// 言語ID
        /// </summary>
        public enum LanguageId
        {
            En,
            Ja,
            AutoDetect
        }

        /// <summary>
        /// テーマID
        /// </summary>
        public enum ThemeId
        {
            Light,
            Dark
        }

        // ======================== マップ定義 ========================

        /// <summary>
        /// UIスケールマップ
        /// </summary>
        private static readonly Dictionary<UiScaleId, double> UiScaleMap = new()
        {
            { UiScaleId.SuperSmall, 0.6 },
            { UiScaleId.Small, 0.8 },
            { UiScaleId.Regular, 1.0 },
            { UiScaleId.Large, 1.05 },
            { UiScaleId.Huge, 1.1 },
        };

        /// <summary>
        /// スクロールホイール感度マップ
        /// </summary>
        private static readonly Dictionary<ScrollWheelSensitivityId, double> ScrollWheelSensitivityMap = new()
        {
            { ScrollWheelSensitivityId.SuperSlow, 0.25 },
            { ScrollWheelSensitivityId.Slow, 0.5 },
            { ScrollWheelSensitivityId.Regular, 1.0 },
            { ScrollWheelSensitivityId.Fast, 2.0 },
            { ScrollWheelSensitivityId.SuperFast, 4.0 },
        };

        /// <summary>
        /// 移動速度マップ
        /// </summary>
        private static readonly Dictionary<MovementSpeedId, double> MovementSpeedMap = new()
        {
            { MovementSpeedId.SuperSlow, 0.25 },
            { MovementSpeedId.Slow, 0.5 },
            { MovementSpeedId.Regular, 1.0 },
            { MovementSpeedId.Fast, 2.0 },
            { MovementSpeedId.SuperFast, 4.0 },
            { MovementSpeedId.ExtremelyFast, 8.0 },
        };

        /// <summary>
        /// オートセーブ間隔マップ
        /// </summary>
        private static readonly Dictionary<AutosaveIntervalId, int?> AutoSaveIntervalMap = new()
        {
            { AutosaveIntervalId.OneMinute, 60 },
            { AutosaveIntervalId.TwoMinutes, 120 },
            { AutosaveIntervalId.FiveMinutes, 300 },
            { AutosaveIntervalId.TenMinutes, 600 },
            { AutosaveIntervalId.TwentyMinutes, 1200 },
            { AutosaveIntervalId.Disabled, null },
        };

        /// <summary>
        /// 言語マップ
        /// </summary>
        public static readonly Dictionary<LanguageId, (string? Name, string? Code, string? Region)> LanguageMap = new()
        {
            { LanguageId.En, ("English", "en", "") },
            { LanguageId.Ja, ("日本語", "ja", "") },
            { LanguageId.AutoDetect, (null, null, null) },
        };

        /// <summary>
        /// テーママップ
        /// </summary>
        public static readonly Dictionary<ThemeId, (string Label, bool IsDark)> ThemeMap = new()
        {
            { ThemeId.Light, ("Light", false) },
            { ThemeId.Dark, ("Dark", true) },
        };

        // ======================== プロパティ定義 ========================

        /// <summary>
        /// 現在のUIスケール倍率を取得する
        /// 未知のIDの場合は1.0を返す。
        /// </summary>
        public double UiScaleValue => UiScaleMap.GetValueOrDefault(UiScale, 1.0);

        /// <summary>
        /// 現在のスクロールホイール感度倍率を取得する。
        /// 未知のIDの場合は1.0を返す。
        /// </summary>
        public double ScrollWheelSensitivityValue
            => ScrollWheelSensitivityMap.GetValueOrDefault(ScrollWheelSensitivity, 1.0);

        /// <summary>
        /// 現在の移動速度倍率を取得する。
        /// 未知のIDの場合は1.0を返す。
        /// </summary>
        public double MovementSpeedValue
            => MovementSpeedMap.GetValueOrDefault(MovementSpeed, 1.0);

        /// <summary>
        /// 現在のオートセーブ間隔（秒）を取得する。
        /// 無効または未知のIDの場合は120秒を返す。
        /// </summary>
        public int GetAutosaveIntervalSeconds()
        {
            if (AutoSaveIntervalMap.TryGetValue(AutoSaveInterval, out var seconds))
                return seconds ?? 120; // Disabledまたはnullならデフォルト120秒

            return 120;
        }

        public UiScaleId UiScale { get; set; } = UiScaleId.Regular;
        public ScrollWheelSensitivityId ScrollWheelSensitivity { get; set; } = ScrollWheelSensitivityId.Regular;
        public MovementSpeedId MovementSpeed { get; set; } = MovementSpeedId.Regular;
        public AutosaveIntervalId AutoSaveInterval { get; set; } = AutosaveIntervalId.TwoMinutes;
        public ThemeId Theme { get; set; } = ThemeId.Light;
        public LanguageId Language { get; set; } = LanguageId.AutoDetect;
        public bool Fullscreen { get; set; } = true;

        public double SoundVolume { get; set; } = 1.0;
        public double MusicVolume { get; set; } = 1.0;

        public int RefreshRate { get; set; } = 60;

        public bool AlwaysMultiplace { get; set; } = false;
        public bool ShapeTooltipAlwaysOn { get; set; } = false;
        public bool OfferHints { get; set; } = true;
        public bool EnableTunnelSmartplace { get; set; } = true;
        public bool Vignette { get; set; } = true;
        public bool CompactBuildingMenu { get; set; } = false;
        public bool DisableCutDeleteWarning { get; set; } = false;
        public bool RotationByBuilding { get; set; } = true;
        public bool ClearCursorOnDeleteWhilePlacing { get; set; } = true;
        public bool DisplayChunkBorders { get; set; } = false;
        public bool PickMinerOnPatch { get; set; } = true;
        public bool EnableMousePan { get; set; } = true;

        public bool EnableColorBlindHelper { get; set; } = false;

        public bool LowQualityMapResources { get; set; } = false;
        public bool DisableTileGrid { get; set; } = false;
        public bool LowQualityTextures { get; set; } = false;
        public bool SimplifiedBelts { get; set; } = false;
        public bool ZoomToCursor { get; set; } = true;
        public double MapResourcesScale { get; set; } = 0.5;

        public Dictionary<string, int> KeybindingOverrides { get; set; } = new();
    }
}
