using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Quaver.API.Enums;
using Quaver.Audio;
using Quaver.Logging;
using Quaver.Modifiers.Mods;
using Quaver.Modifiers.Mods.Mania;

namespace Quaver.Modifiers
{
    /// <summary>
    ///     Entire class that controls the addition and removal of game mods.
    /// </summary>
    internal static class ModManager
    {
        /// <summary>
        ///     The list of currently activated game modifiers.
        /// </summary>
        public static List<IGameplayModifier> CurrentModifiersList { get; } = new List<IGameplayModifier>();

        /// <summary>
        ///     The current modifiers in ModId format
        /// </summary>
        public static ModIdentifier Mods
        {
            get
            {
                var mods = 0;

                foreach (var mod in CurrentModifiersList)
                    mods += (int) mod.ModIdentifier;

                return (ModIdentifier) mods;
            }
        }

        /// <summary>
    ///     Adds a gameplayModifier to our list, getting rid of any incompatible mods that are currently in there.
    ///     Also, specifying a speed, if need-be. That is only "required" if passing in ModIdentifier.ManiaModSpeed
    /// </summary>
    public static void AddMod(ModIdentifier modIdentifier)
        {
            IGameplayModifier gameplayModifier;

            // Set the newMod based on the ModType that is coming in
            switch (modIdentifier)
            {
                case ModIdentifier.Speed05X:                  
                case ModIdentifier.Speed06X:
                case ModIdentifier.Speed07X:
                case ModIdentifier.Speed08X:
                case ModIdentifier.Speed09X:
                case ModIdentifier.Speed11X:
                case ModIdentifier.Speed12X:
                case ModIdentifier.Speed13X:
                case ModIdentifier.Speed14X:
                case ModIdentifier.Speed15X:
                case ModIdentifier.Speed16X:
                case ModIdentifier.Speed17X:
                case ModIdentifier.Speed18X:
                case ModIdentifier.Speed19X:
                case ModIdentifier.Speed20X:
                    gameplayModifier = new ManiaModSpeed(modIdentifier);
                    break;
                case ModIdentifier.NoSliderVelocity:
                    gameplayModifier = new ManiaModNoSliderVelocities();
                    break;
                case ModIdentifier.Strict:
                    gameplayModifier = new ManiaModStrict();
                    break;
                case ModIdentifier.Chill:
                    gameplayModifier = new ManiaModChill();
                    break;
                case ModIdentifier.NoPause:
                    gameplayModifier = new ManiaModNoPause();
                    break;
                case ModIdentifier.Autoplay:
                    gameplayModifier = new ManiaModAutoplay();
                    break;
                default:
                    return;
            }
            
            // Remove incompatible mods.
            var incompatibleMods = CurrentModifiersList.FindAll(x => x.IncompatibleMods.Contains(gameplayModifier.ModIdentifier));
            incompatibleMods.ForEach(x => RemoveMod(x.ModIdentifier));

            // Add The Mod
            CurrentModifiersList.Add(gameplayModifier);
            gameplayModifier.InitializeMod();  
        }

        /// <summary>
        ///     Removes a gameplayModifier from our GameBase
        /// </summary>
        public static void RemoveMod(ModIdentifier modIdentifier)
        {
            try
            {
                // Try to find the removed gameplayModifier in the list
                var removedMod = CurrentModifiersList.Find(x => x.ModIdentifier == modIdentifier);

                // Remove the Mod
                CurrentModifiersList.Remove(removedMod);
            }
            catch (Exception e)
            {
                Logger.LogError(e, LogType.Runtime);
            }
        }

        /// <summary>
        ///     Checks if a gameplayModifier is currently activated.
        /// </summary>
        /// <param name="modIdentifier"></param>
        /// <returns></returns>
        public static bool IsActivated(ModIdentifier modIdentifier) => CurrentModifiersList.Exists(x => x.ModIdentifier == modIdentifier);

        /// <summary>
        ///     Removes all items from our list of mods
        /// </summary>
        public static void RemoveAllMods()
        {
            CurrentModifiersList.Clear();
            CheckModInconsistencies();
        }

        /// <summary>
        ///     Removes any speed mods from the game and resets the clock
        /// </summary>
        public static void RemoveSpeedMods()
        {
            try
            {
                CurrentModifiersList.RemoveAll(x => x.Type == ModType.Speed);
                CheckModInconsistencies();
            }
            catch (Exception e)
            {
                Logger.LogError(e, LogType.Runtime);
            }
        }

        /// <summary>
        ///     Makes sure that the speed gameplayModifier selected matches up with the game clock and sets the correct one.
        /// </summary>
        public static void CheckModInconsistencies()
        {
            if (Mods == 0)
                return;

            var mod = CurrentModifiersList.Find(x => x.Type == ModType.Speed);

            if (mod == null)
                return;

            // Re-intialize the correct gameplayModifier.
            var index = CurrentModifiersList.IndexOf(mod);

            if (index != -1)
                CurrentModifiersList[index] = new ManiaModSpeed(mod.ModIdentifier);
            else
                AudioEngine.Track.Rate = 1.0f;
        }
    }
}
