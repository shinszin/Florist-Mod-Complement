using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using StardewValley.Network;

namespace ButterflySpawner
{
    public class ModEntry : Mod
    {
        public override void Entry(IModHelper helper)
        {
            // Register the event handler for Warped
            helper.Events.Player.Warped += OnWarped;
            //Monitor.Log("ModEntry.Entry called and Warped event subscribed.", LogLevel.Info);
        }

        private void OnWarped(object sender, WarpedEventArgs e)
        {
            // Check if the player has warped to the Flower Shop
            if (e.NewLocation != null && e.NewLocation.Name == "FlowerShop")
            {
                // Define specific tile positions where butterflies will spawn
                List<Vector2> butterflyPositions = new List<Vector2>
                {
                    new Vector2(1, 9),
                    new Vector2(11, 8),
                    new Vector2(2, 5)
                };

                Vector2 frogPosition = new Vector2(6,5);


                // Spawn butterflies at the predefined positions in the Flower Shop
                SpawnButterflies(e.NewLocation, butterflyPositions);
                SpawnFrog(e.NewLocation, frogPosition);

                // Log the event
                //Monitor.Log($"Spawned butterflies at the Flower Shop. Location: {e.NewLocation.Name}", LogLevel.Info);
            }
        }

        private void SpawnButterflies(GameLocation location, List<Vector2> tilePositions)
        {
            try
            {
                // Check if location is valid
                if (location == null)
                {
                    Monitor.Log("Failed to spawn butterflies. Location is null.", LogLevel.Error);
                    return;
                }

                // Log to check if the method is called and the parameters are correct
                //Monitor.Log($"Spawning butterflies at location: {location.Name}", LogLevel.Trace);

                // Define the scale and other properties
                int zoom = 3;

                // Define source rectangles and animation lengths for different butterflies on the tilesheet
                List<(Rectangle sourceRect, int animationLength)> butterflies = new List<(Rectangle sourceRect, int animationLength)>
                {
                    (new Rectangle(128, 96, 16, 16), 4), // Butterfly 1
                    (new Rectangle(96, 144, 16, 16), 3),  // Butterfly 2
                    (new Rectangle(272, 304, 16, 16), 3), // Butterfly 3
                }

                Random rand = new Random();

                for (int i = 0; i < tilePositions.Count; i++)
                {
                    Vector2 tilePosition = tilePositions[i];
                    Vector2 butterflyPosition = new Vector2(tilePosition.X * Game1.tileSize, tilePosition.Y * Game1.tileSize);

                    // Choose a source rectangle and animation length for each butterfly
                    (Rectangle sourceRect, int animationLength) = butterflies[i % butterflies.Count];

                    // Create a new TemporaryAnimatedSprite instance representing a butterfly
                    TemporaryAnimatedSprite butterfly = new TemporaryAnimatedSprite("TileSheets\\critters", sourceRect, butterflyPosition, false, 0f, Color.White)
                    {
                        scale = zoom,
                        animationLength = animationLength,
                        totalNumberOfLoops = 999999,
                        pingPong = true,
                        interval = 75f,
                        local = false,
                        yPeriodic = true,
                        yPeriodicLoopTime = 3200f,
                        yPeriodicRange = 10f,
                        xPeriodic = true,
                        xPeriodicLoopTime = 5000f,
                        xPeriodicRange = 8f,
                        alpha = 1f // Full opacity
                    };

                    // Add the butterfly to the location's temporary sprites list
                    location.TemporarySprites.Add(butterfly);

                    // Log the result
                    //Monitor.Log($"Successfully spawned butterfly at location: {location.Name}, Position: {tilePosition}, Source Rect: {sourceRect}, Animation Length: {animationLength}", LogLevel.Debug);
                }
            }
            catch (Exception ex)
            {
                Monitor.Log($"An error occurred while spawning butterflies: {ex}", LogLevel.Error);
            }
        }

        private void SpawnFrog(GameLocation location, Vector2 tilePosition)
        {
            try
            {
                // Define the scale and other properties for frogs
                int zoom = 3;

                // Calculate the position for the frog
                Vector2 frogPosition = new Vector2(tilePosition.X * Game1.tileSize, tilePosition.Y * Game1.tileSize);

                // Create a new TemporaryAnimatedSprite instance representing a frog
                TemporaryAnimatedSprite frog = new TemporaryAnimatedSprite("TileSheets\\critters", new Rectangle(65, 241, 16, 16), frogPosition, false, 0f, Color.White)
                {
                    scale = zoom,
                    animationLength = 3,
                    totalNumberOfLoops = 999999,
                    pingPong = false, // Frogs don't need ping pong animation
                    interval = 1000f, // Interval can be longer for frogs since they don't move
                    local = false, 
                    alpha = 1f // Full opacity
                };

                // Add the frog to the location's temporary sprites list
                location.TemporarySprites.Add(frog);

                // Log the result
                //Monitor.Log($"Successfully spawned frog at location: {location.Name}, Position: {tilePosition}", LogLevel.Debug);
            }
            catch (Exception ex)
            {
                Monitor.Log($"An error occurred while spawning frog: {ex}", LogLevel.Error);
            }
        }
    }
}
