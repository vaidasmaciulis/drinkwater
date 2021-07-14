using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using System.Collections.Generic;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.ModAPI;
using VRage.Utils;

namespace DrinkWater
{
	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
	public class Server : MySessionComponentBase
	{
		private const int SKIP_TICKS = 300;
		private const float WATER_USAGE = 0.1f;
		private const float WATER_DAMAGE = 0.1f;
		
		private static List<IMyPlayer> players = new List<IMyPlayer>();
		private static int skipTick = 0;

        public override void UpdateAfterSimulation()
        {
			if (skipTick++ <= SKIP_TICKS)
            {
				return;
            } 
			else
            {
				skipTick = 0;
			}

			players.Clear();
			MyAPIGateway.Players.GetPlayers(players);

			foreach (IMyPlayer player in players)
            {
				IMyEntity entity = player.Controller.ControlledEntity.Entity;

				MyEntityStatComponent statComp;
				entity.Components.TryGet(out statComp);
				MyEntityStat water;
				statComp.TryGetStat(MyStringHash.GetOrCompute("Water"), out water);

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
    }
}
