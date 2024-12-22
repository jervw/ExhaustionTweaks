using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using ExhaustionTweaks.Compatibility;

namespace ExhaustionTweaks
{
    /// <summary>The mod entry point.</summary>
    internal sealed class ModEntry : Mod
    {
        /*********
        ** Fields
        *********/
        /// <summary>The mod configuration.</summary>
        private ModConfig? _modConfig;

        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            _modConfig = helper.ReadConfig<ModConfig>();

            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            helper.Events.GameLoop.UpdateTicked += BathHousePoolCheck;
        }

        /*********
        ** Private methods
        *********/
        /// <summary>Event handler for the game launching.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
        {
            var configMenu = Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (configMenu != null && _modConfig != null)
            {
                configMenu.Register(ModManifest, () => _modConfig = new ModConfig(), () => Helper.WriteConfig(_modConfig));
                configMenu.AddSectionTitle(
                    mod: ModManifest,
                    text: () => "Remove exhaustion:"
                );
                configMenu.AddBoolOption(
                    mod: ModManifest,
                    name: () => "Bath House",
                    tooltip: () => "Remove exhaustion in Bath House Pool",
                    getValue: () => _modConfig.RemoveExhaustionInBathHousePool,
                    setValue: value => _modConfig.RemoveExhaustionInBathHousePool = value
                );
                Monitor.Log("Loaded API for mod GenericModConfigMenu", LogLevel.Debug);
            }
        }


        /// <summary>
        /// Check if the player is swimming in the Bath House Pool and if they are at full stamina, remove exhaustion.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void BathHousePoolCheck(object? sender, UpdateTickedEventArgs e)
        {
            if (!Context.IsWorldReady || _modConfig == null || !_modConfig.RemoveExhaustionInBathHousePool)
                return;

            if (e.IsMultipleOf(60))
            {
                if (Game1.player.currentLocation.Name == "BathHouse_Pool")
                {
                    bool isPlayerSwimming = Game1.player.swimming.Value;
                    if (isPlayerSwimming)
                    {
                        if ((Game1.player.Stamina >= Game1.player.MaxStamina) && Game1.player.exhausted.Value)
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
