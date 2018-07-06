﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Quaver.Graphics;
using Quaver.Graphics.Buttons;
using Quaver.Graphics.Sprites;
using Quaver.Graphics.Text;
using Quaver.Helpers;
using Quaver.Main;

namespace Quaver.States.Menu
{
    internal class NavigationButton : Button
    {
        /// <summary>
        ///     The header border.
        /// </summary>
        private Sprite Header { get; }

        /// <summary>
        ///     The text in the header
        /// </summary>
        private SpriteText HeaderText { get; }

        /// <summary>
        ///     The actual image to be displayed for this button.
        /// </summary>
        private Sprite ButtonImage { get; }

        /// <summary>
        ///     Keeps track of if the hover sound has played.
        /// </summary>
        private bool HoverSoundPlayed { get; set; }

        /// <summary>
        ///     The background of the footer.
        /// </summary>
        private Sprite FooterBackground { get; set; }

        /// <summary>
        ///     The actual footer text that describes the button. 
        /// </summary>
        private SpriteText FooterText { get;  }

        /// <inheritdoc />
        /// <summary>
        ///     Ctor -
        /// </summary>
        /// <param name="size"></param>
        /// <param name="headerText"></param>
        /// <param name="image"></param>
        internal NavigationButton(Vector2 size, string headerText, Texture2D image, string footerText = null)
        {
            Size = new UDim2D(size.X, size.Y);
            
            Header = new Sprite
            {
                Parent = this,
                Alignment = Alignment.TopLeft,
                Tint = Colors.DarkGray,
                Size = new UDim2D(SizeX, 45)
            };

            HeaderText = new SpriteText
            {
                Parent = Header,
                Alignment = Alignment.MidLeft,
                TextAlignment = Alignment.MidLeft,
                Font = Fonts.Exo2Regular24,
                Text = headerText,
                TextScale = 0.58f,
                PosX = 20
            };

            ButtonImage = new Sprite
            {
                Parent = this,
                Alignment = Alignment.TopLeft,
                Size = new UDim2D(SizeX, SizeY - Header.SizeY),
                PosY = Header.SizeY,
                Image = image
            };

            if (footerText == null)
                return;
            
            FooterBackground = new Sprite()
            {
                Parent = this,
                Alignment = Alignment.BotLeft,
                Size = new UDim2D(SizeX, 40),
                Tint = Color.Black,
                Alpha = 0.70f
            };

            FooterText = new SpriteText()
            {
                Parent = FooterBackground,
                Alignment = Alignment.MidCenter,
                TextAlignment = Alignment.MidCenter,
                Font = Fonts.Exo2Regular24,
                Text = footerText,
                TextScale = 0.42f
            };
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        protected override void OnClicked()
        {
            GameBase.AudioEngine.PlaySoundEffect(GameBase.Skin.SoundClick);
            base.OnClicked();
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        protected override void MouseOut()
        {
            HoverSoundPlayed = false;
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        protected override void MouseOver()
        {
            if (!HoverSoundPlayed)
            {
                GameBase.AudioEngine.PlaySoundEffect(GameBase.Skin.SoundHover);
                HoverSoundPlayed = true;
            }
        }
    }
}