using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System.Collections.Generic;
using System.Linq;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Utils;

namespace DrinkWater
{
    [MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
    public class Session : MySessionComponentBase
    {
        private const int TICKS_TO_SKIP = 300;
        private const float WATER_USAGE = 0.1f;
        private const float WATER_DAMAGE = 0.5f;

        private static List<IMyPlayer> players = new List<IMyPlayer>();
        private static int skippedTicks = 0;

        public override void UpdateAfterSimulation()
        {
            if (skippedTicks++ < TICKS_TO_SKIP)
            {
                return;
            }
            skippedTicks = 0;

            players.Clear();
            MyAPIGateway.Players.GetPlayers(players);

            foreach (IMyPlayer player in players)
            {
                MyInventoryBase inventory = (MyInventoryBase)player.Character.GetInventory();
                MyEntityStat water = GetPlayerWaterStat(player);
                inventory.ContentsRemoved += (item, point) =>
                {
                    string objectIdString = item.Content.GetObjectId().ToString();
                    string[] drinks = {
                        "ClangCola",
                        "CosmicCoffee",
                        "SparklingWater"
                    };

                    if (drinks.Any(drink => objectIdString.Contains(drink)))
                    {
                        //Make sure it was not just removing drinks from inventory
                        if (water.HasAnyEffect())
                        {
                            if (player.Character.EnabledHelmet)
                            {
                                player.Character.SwitchHelmet();
                                MyAPIGateway.Utilities.ShowMessage("DrinkWater", "Had to open helmet to Drink!");
                            }
                        }
                    }
                };

                if (water.Value > 0)
                {
                    water.Decrease(WATER_USAGE, null);
                }

                if (water.Value <= 0)
                {
                    player.Character.DoDamage(WATER_DAMAGE, MyStringHash.GetOrCompute("Unknown"), true);
                }
            }
        }

        private MyEntityStat GetPlayerWaterStat(IMyPlayer player)
        {
            MyEntityStatComponent statComp;
            player.Character.Components.TryGet(out statComp);

            MyEntityStat water;
            statComp.TryGetStat(MyStringHash.GetOrCompute("Water"), out water);
            return water;
        }
    }
}
