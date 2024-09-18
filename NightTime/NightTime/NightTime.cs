using System.Reflection;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace NightTimeMod
{
    public class ModEntry : Mod
    {
        private Color dayColor = new Color(50, 50, 50);
        private Color nightColor = new Color(150, 150, 30);

        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.TimeChanged += OnTimeChanged;
            helper.Events.Player.Warped += OnWarped;
        }

        private void OnTimeChanged(object? sender, TimeChangedEventArgs e)
        {
            // Get the FlowerShop location
            var flowerShop = Game1.getLocationFromName("FlowerShop");
            if (flowerShop != null)
            {
                // Set custom lighting colors
                SetCustomLighting(flowerShop, dayColor, nightColor);
            }
        }

        private void OnWarped(object? sender, WarpedEventArgs e)
        {
            // Check if the player warped to the FlowerShop
            if (e.NewLocation.Name == "FlowerShop")
            {
                // Set custom lighting colors
                SetCustomLighting(e.NewLocation, dayColor, nightColor);
            }
        }

        private void SetCustomLighting(GameLocation location, Color dayColor, Color nightColor)
        {
            // Use reflection to set the protected fields
            FieldInfo indoorLightingColorField = typeof(GameLocation).GetField("indoorLightingColor", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo indoorLightingNightColorField = typeof(GameLocation).GetField("indoorLightingNightColor", BindingFlags.NonPublic | BindingFlags.Instance);

            if (indoorLightingColorField != null)
            {
                indoorLightingColorField.SetValue(location, dayColor);
            }

            if (indoorLightingNightColorField != null)
            {
                indoorLightingNightColorField.SetValue(location, nightColor);
            }
        }
    }
}
