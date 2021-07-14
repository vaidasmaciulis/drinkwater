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
