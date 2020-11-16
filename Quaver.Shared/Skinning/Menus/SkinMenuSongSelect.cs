﻿using IniFileParser.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Quaver.Shared.Config;
using Wobble.Graphics;

namespace Quaver.Shared.Skinning.Menus
{
    public class SkinMenuSongSelect : SkinMenu
    {
        public bool DisplayMapBackground { get; private set; }

        public byte? MapBackgroundBrightness { get; private set; }

        public Color? MapsetPanelSongTitleColor { get; private set; }

        public Color? MapsetPanelSongArtistColor { get; private set; }

        public Color? MapsetPanelCreatorColor { get; private set; }

        public Color? MapsetPanelByColor { get; private set; }

        public ScalableVector2? MapsetPanelBannerSize { get; private set; }

        public Texture2D MapsetSelected { get; private set; }

        public Texture2D MapsetDeselected { get; private set; }

        public Texture2D MapsetHovered { get; private set; }

        public Texture2D GameMode4K { get; private set; }

        public Texture2D GameMode7K { get; private set; }

        public Texture2D GameMode4K7K { get; private set; }

        public Texture2D StatusNotSubmitted { get; private set; }

        public Texture2D StatusUnranked { get; private set; }

        public Texture2D StatusRanked { get; private set; }

        public Texture2D StatusOsu { get; private set; }

        public Texture2D StatusStepmania { get; private set; }

        public SkinMenuSongSelect(SkinStore store, IniData config) : base(store, config)
        {
        }

        protected override void ReadConfig()
        {
            var ini = Config["SongSelect"];

            var displayMapBackground = ini["DisplayMapBackground"];
            ReadIndividualConfig(displayMapBackground, () => DisplayMapBackground = ConfigHelper.ReadBool(false, displayMapBackground));

            var mapBackgroundBrightness = ini["MapBackgroundBrightness"];
            ReadIndividualConfig(mapBackgroundBrightness, () => MapBackgroundBrightness = ConfigHelper.ReadByte(0, mapBackgroundBrightness));

            var mapsetPanelSongTitleColor = ini["MapsetPanelSongTitleColor"];
            ReadIndividualConfig(mapsetPanelSongTitleColor, () => MapsetPanelSongTitleColor = ConfigHelper.ReadColor(Color.Transparent, mapsetPanelSongTitleColor));

            var mapsetPanelSongArtistColor = ini["MapsetPanelSongArtistColor"];
            ReadIndividualConfig(mapsetPanelSongArtistColor, () => MapsetPanelSongArtistColor = ConfigHelper.ReadColor(Color.Transparent, mapsetPanelSongArtistColor));

            var mapsetPanelCreatorColor = ini["MapsetPanelCreatorColor"];
            ReadIndividualConfig(mapsetPanelCreatorColor, () => MapsetPanelCreatorColor = ConfigHelper.ReadColor(Color.Transparent, mapsetPanelCreatorColor));

            var mapsetPanelByColor = ini["MapsetPanelByColor"];
            ReadIndividualConfig(mapsetPanelByColor, () => MapsetPanelByColor = ConfigHelper.ReadColor(Color.Transparent, mapsetPanelByColor));

            var mapsetPanelBannerSize = ini["MapsetPanelBannerSize"];
            ReadIndividualConfig(mapsetPanelBannerSize, () => MapsetPanelBannerSize = ConfigHelper.ReadVector2(new ScalableVector2(0, 0), mapsetPanelBannerSize));
        }

        protected override void LoadElements()
        {
            const string folder = "SongSelect";

            MapsetSelected = LoadSkinElement(folder, "mapset-selected.png");
            MapsetDeselected = LoadSkinElement(folder, "mapset-deselected.png");
            MapsetHovered = LoadSkinElement(folder, "mapset-hovered.png");
            GameMode4K = LoadSkinElement(folder, "game-mode-4k.png");
            GameMode7K = LoadSkinElement(folder, "game-mode-7k.png");
            GameMode4K7K = LoadSkinElement(folder, "game-mode-4k7k.png");
            StatusNotSubmitted = LoadSkinElement(folder, "status-notsubmitted.png");
            StatusUnranked = LoadSkinElement(folder, "status-unranked.png");
            StatusRanked = LoadSkinElement(folder, "status-ranked.png");
            StatusOsu = LoadSkinElement(folder, "status-osu.png");
            StatusStepmania = LoadSkinElement(folder, "status-sm.png");
        }
    }
}