using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.ModAPI;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.ModAPI;
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

        public override void BeforeStart()
        {
            base.BeforeStart();

            players.Clear();
            MyAPIGateway.Players.GetPlayers(players);
            foreach (IMyPlayer player in players)
            {
                MyInventoryBase inventory = (MyInventoryBase)player.Character.GetInventory();
                inventory.ContentsRemoved += (item, point) =>
                {
                    string objectIdString = item.Content.GetObjectId().ToString();
                    if (objectIdString.Contains("ClangCola") || objectIdString.Contains("CosmicCoffee"))
                    {
                        MyEntityStat water = GetPlayerWaterStat(player);

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
            }
        }

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
                IMyEntity entity = player.Controller.ControlledEntity.Entity;

                MyEntityStat water = GetPlayerWaterStat(player);

                MyDefinitionId waterId = new MyDefinitionId(typeof(MyObjectBuilder_GasProperties), "Water");
                float waterLevel = player.Character.GetSuitGasFillLevel(waterId);

                //TODO MyCharacter is prohibited, need to find workaround
                (player.Character as MyCharacter).UpdateStoredGas(waterId, waterLevel - WATER_USAGE);

                if (water.Value > 0)
                {
                    water.Decrease(WATER_USAGE, null);
                }

                if (water.Value <= 0)
                {
                    var destroyable = entity as IMyDestroyableObject;
                    destroyable.DoDamage(WATER_DAMAGE, MyStringHash.GetOrCompute("Unknown"), true);
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
