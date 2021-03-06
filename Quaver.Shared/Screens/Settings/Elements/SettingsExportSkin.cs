/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 * Copyright (c) Swan & The Quaver Team <support@quavergame.com>.
*/

using Quaver.Shared.Assets;
using Quaver.Shared.Graphics;
using Quaver.Shared.Screens.Menu.UI.Navigation.User;
using Quaver.Shared.Skinning;
using Wobble.Assets;
using Wobble.Graphics;
using Wobble.Graphics.UI.Buttons;

namespace Quaver.Shared.Screens.Settings.Elements
{
    public class SettingsExportSkin : SettingsItem
    {
        /// <summary>
        ///     The button used to export the skin
        /// </summary>
        private BorderedTextButton ExportButton { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="dialog"></param>
        /// <param name="name"></param>
        public SettingsExportSkin(SettingsDialog dialog, string name) : base(dialog, name) => CreateExportButton();

        /// <summary>
        /// </summary>
        private void CreateExportButton()
        {
            ExportButton = new BorderedTextButton("Export Skin", Colors.MainAccent)
            {
                Parent = this,
                X = -50,
                Alignment = Alignment.MidRight,
                Height = 30,
                Width = 225,
                Text =
                {
                    Font = Fonts.SourceSansProSemiBold,
                    FontSize = 12
                }
            };

            ExportButton.Clicked += (o, e) => SkinManager.Export();
        }
    }
}
