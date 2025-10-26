using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;

namespace ShapezMono.Game.profile
{
    public class SettingsStorage
    {
        public string UiScale { get; set; } = "regular";
        public bool Fullscreen { get; set; } = true;

        public double SoundVolume { get; set; } = 1.0;
        public double MusicVolume { get; set; } = 1.0;

        public string Theme { get; set; } = "light";
        public string RefreshRate { get; set; } = "60";
        public string ScrollWheelSensitivity { get; set; } = "regular";
        public string Language { get; set; } = "auto-detect";
        public string AutoSaveInterval { get; set; } = "two-minutes";

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
