using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace ExhaustionTweaks
{
    /// <summary>The mod entry point.</summary>
    internal sealed class ModEntry : Mod
    {
        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.UpdateTicked += this.BathHousePoolCheck;
        }

        /*********
        ** Private methods
        *********/
        /// <summary>
        /// Check if the player is swimming in the Bath House Pool and if they are at full stamina, remove exhaustion.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BathHousePoolCheck(object? sender, UpdateTickedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            if (e.IsMultipleOf(60))
            {
                if (Game1.player.currentLocation.Name == "BathHouse_Pool")
                {
                    bool isPlayerSwimming = Game1.player.swimming.Value;
                    if (isPlayerSwimming)
                    {
                        if (Game1.player.Stamina >= Game1.player.MaxStamina)
                        {
                            Game1.player.exhausted.Value = false;
                            Game1.showGlobalMessage("You are no longer exhausted!");
                        }
                    }
                }
            }
        }

    }
}
